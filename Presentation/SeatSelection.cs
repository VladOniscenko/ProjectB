using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SeatSelection
{
    public readonly Showtime? SelectedShowtime;
    public readonly Movie? SelectedMovie;
    public readonly Auditorium? SelectedAuditorium;
    public readonly IEnumerable<SeatReservation>? ReservedSeats;
    public readonly IEnumerable<Seat>? Seats;
    public readonly List<int>? ReservedSeatIds;

    public SeatSelection(int showtimeId)
    {
        SelectedShowtime = ShowtimeLogic.Find(showtimeId);
        if (SelectedShowtime == null)
        {
            ConsoleMethods.Error("Showtime was not found!");
            return;
        }

        SelectedMovie = MovieLogic.Find(SelectedShowtime.MovieId);
        SelectedAuditorium = AuditoriumLogic.Find(SelectedShowtime.AuditoriumId);
        if (SelectedMovie == null || SelectedAuditorium == null)
        {
            ConsoleMethods.Error("Movie or Auditorium was not found!");
            return;
        }
        
        Seats = SeatLogic.GetSeatsByAuditorium(SelectedAuditorium.Id);
        if (Seats == null || Seats.Count() <= 0)
        {
            ConsoleMethods.Error("Seats not found!");
            return;
        }
        
        ReservedSeats = SeatReservationLogic.GetReservedSeatsByShowtimeId(SelectedShowtime.Id);
        ReservedSeatIds = ReservedSeats.Select(s => s.SeatId).ToList();
    }
    public void SelectSeats(int showtimeId)
    {
        Seat? currentSeat = null;
        while (true)
        {
            Console.Clear();
            PrintSeatSelectionRules();

            int? currentRow = null;
            foreach (var seat in Seats)
            {
                if (currentRow != seat.Row)
                {
                    if (currentRow != null)
                        Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{seat.Row:D2} |     ");
                    Console.ResetColor();
                    currentRow = seat.Row;
                }

                string type = "DS";
                if (seat.Type == "love_seat")
                {
                    type = "LV";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (seat.Type == "vip")
                {
                    type = "VP";
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (seat.Type == "normal")
                {
                    type = "NO";
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                if (seat.Active == 1)
                {
                    if (currentSeat == null)
                    {
                        currentSeat = seat;
                    }

                    if (currentSeat.Id == seat.Id)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                }

                string seatContent = " ";
                if (ReservedSeatIds.Contains(seat.Id))
                {
                    seatContent = "X";
                }
                
                Console.Write(seat.Active == 1 ? $"[{seatContent}]" : "   ");
                Console.ResetColor();
                Console.Write(" ");
            }
            
            Console.WriteLine();
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            if (pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.D)
            {
                var nextSeat = Seats.FirstOrDefault(s => s.Row == currentSeat.Row && s.Number == currentSeat.Number + 1 && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.A)
            {
                var nextSeat = Seats.FirstOrDefault(s => s.Row == currentSeat.Row && s.Number == currentSeat.Number - 1 && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow || pressedKey.Key == ConsoleKey.S)
            {
                var nextSeat = Seats.FirstOrDefault(s => s.Row == currentSeat.Row - 1 && s.Number == currentSeat.Number && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.W)
            {
                var nextSeat = Seats.FirstOrDefault(s => s.Row == currentSeat.Row + 1 && s.Number == currentSeat.Number && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
        }
    }
    
    static void PrintSeatSelectionRules()
    {
        Console.WriteLine("Rules:");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[X]");
        Console.ResetColor();
        Console.WriteLine(" - Reserved (already taken)");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[ ]");
        Console.ResetColor();
        Console.WriteLine(" - Available (free)");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[S]");
        Console.ResetColor();
        Console.WriteLine(" - Selected (your choice)");

        Console.WriteLine();
    }

}