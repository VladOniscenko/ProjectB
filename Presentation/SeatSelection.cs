using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SeatSelection
{
    public readonly Showtime? SelectedShowtime;
    public readonly Movie? SelectedMovie;
    public readonly IEnumerable<Seat>? Seats;
    public Seat? SelectedSeat {get; private set;} = null;
    
    public SeatSelection(int showtimeId)
    {
        SelectedShowtime = ShowtimeLogic.Find(showtimeId);
        if (SelectedShowtime == null)
        {
            ConsoleMethods.Error("Showtime was not found!");
            return;
        }

        SelectedMovie = MovieLogic.Find(SelectedShowtime.MovieId);
        Seats = SeatLogic.GetSeatsByShowtime(SelectedShowtime.Id);
        if (SelectedMovie == null || Seats == null || Seats.Count() <= 0)
        {
            ConsoleMethods.Error("Something went wrong! Get in touch with our Customer service!");
            return;
        }
        
        // get first active seat
        SelectedSeat = Seats.FirstOrDefault(s => s.Active == 1);
    }
    public void SelectSeats()
    {
        while (true)
        {
            Console.Clear();
            PrintSeatSelectionContent();
            
            // display all seats in the console
            int? currentRow = null;
            foreach (var seat in Seats)
            {
                // draw row number if not the same as seat row
                if (currentRow != seat.Row)
                {
                    if (currentRow != null)
                    {
                        Console.WriteLine();
                    }
                    currentRow = seat.Row;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{seat.Row:D2} |     ");
                    Console.ResetColor();
                }
                
                ConsoleColor? seatColor = seat.Type switch
                {
                    "love_seat" => ConsoleColor.Yellow,
                    "vip" => ConsoleColor.Red,
                    "normal" => ConsoleColor.Blue,
                    _ => null
                };

                // highlight the seat that is currently selected
                if (SelectedSeat.Id == seat.Id)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else if (seat.Selected)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    seatColor = ConsoleColor.Magenta;
                }
                
                if (seatColor != null)
                {
                    Console.ForegroundColor = seatColor.Value;
                }

                // set content of the chair
                // [ ] = available and [X] is taken
                string seatContent = seat.Taken == 1 ? "X" : " ";
                seatContent = seat.Selected ? "S" : seatContent;
                Console.Write(seat.Active == 1 ? $"[{seatContent}]" : "   ");
                Console.ResetColor();
                Console.Write(" ");
            }
            
            Console.WriteLine();
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            if (pressedKey.Key == ConsoleKey.Enter)
            {
                ReservationLogic.AddOrRemoveSeat(SelectedSeat);
            }
            else
            {
                SelectedSeat = Move(pressedKey);   
            }
        }
    }
    
    private void PrintSeatSelectionContent()
    {
        Console.WriteLine("Selected movie:");
        Console.WriteLine(SelectedMovie);
        Console.WriteLine();

        Console.WriteLine("Rules:");
        
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine("[S] - Selected seats");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[X] - Reserved (already taken)");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[ ] - Available (free)");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[S] - Selected (your choice)");

        Console.WriteLine();
        Console.ResetColor();
    }

    private Seat Move(ConsoleKeyInfo pressedKey)
    {
        // decide direction
        int row = pressedKey.Key switch
        {
            (ConsoleKey.UpArrow or ConsoleKey.W) => 1,
            (ConsoleKey.DownArrow or ConsoleKey.S) => -1,
            _ => 0
        };
        
        int number = pressedKey.Key switch
        {
            (ConsoleKey.D or ConsoleKey.RightArrow) => 1,
            (ConsoleKey.LeftArrow or ConsoleKey.A) => -1,
            _ => 0
        };
        
        // search for matching seat
        var nextSeat = Seats.FirstOrDefault(
            s => 
                s.Row == SelectedSeat.Row + row && 
                s.Number == SelectedSeat.Number + number && 
                s.Active == 1
        );
        
        // if seat not found return current seat. otherwise return found seat
        return nextSeat == null ? SelectedSeat : nextSeat;
    }

}