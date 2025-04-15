using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public static class SeatSelection
{
    public static void SelectSeats(int showtimeId)
    {
        Showtime? showtime = ShowtimeLogic.Find(showtimeId);
        if (showtime == null)
        {
            ConsoleMethods.Error("Showtime was not found!");
            return;
        }

        Movie? movie = MovieLogic.Find(showtime.MovieId);
        Auditorium? auditorium = AuditoriumLogic.Find(showtime.AuditoriumId);
        if (movie == null || auditorium == null)
        {
            ConsoleMethods.Error("Movie or Auditorium was not found!");
            return;
        }
        
        // todo retrieve taken seats
        
        IEnumerable<Seat> seats = SeatLogic.GetSeatsByAuditorium(auditorium.Id);
        Seat? currentSeat = null;

        while (true)
        {
            Console.Clear();
            int? currentRow = null;
            foreach (var seat in seats)
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

                Console.Write(seat.Active == 1 ? $"[ ]" : "   ");
                Console.ResetColor();
                Console.Write(" ");
            }
            
            Console.WriteLine();
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            if (pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.D)
            {
                var nextSeat = seats.FirstOrDefault(s => s.Row == currentSeat.Row && s.Number == currentSeat.Number + 1 && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.A)
            {
                var nextSeat = seats.FirstOrDefault(s => s.Row == currentSeat.Row && s.Number == currentSeat.Number - 1 && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow || pressedKey.Key == ConsoleKey.S)
            {
                var nextSeat = seats.FirstOrDefault(s => s.Row == currentSeat.Row - 1 && s.Number == currentSeat.Number && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.W)
            {
                var nextSeat = seats.FirstOrDefault(s => s.Row == currentSeat.Row + 1 && s.Number == currentSeat.Number && s.Active == 1);
                if (nextSeat != null)
                {
                    currentSeat = nextSeat;
                }
            }

        }
    }
}