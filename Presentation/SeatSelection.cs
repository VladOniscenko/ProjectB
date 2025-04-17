using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SeatSelection
{
    public readonly Showtime? SelectedShowtime;
    public readonly Movie? SelectedMovie;
    public readonly Auditorium? SelectedAuditorium;
    public readonly IEnumerable<Seat>? Seats;
    private readonly int MaxNumber;
    private readonly int MaxRow;
    private bool Running = false;
    private int interactiveStartLine;

    public Seat? SelectedSeat { get; private set; } = null;

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
        SelectedAuditorium = AuditoriumLogic.Find(SelectedShowtime.AuditoriumId);
        if (SelectedMovie == null || Seats == null || Seats.Count() <= 0)
        {
            ConsoleMethods.Error("Something went wrong! Get in touch with our Customer service!");
            return;
        }

        // get first active seat
        SelectedSeat = Seats.FirstOrDefault(s => s.Active == 1);

        MaxNumber = Seats.Max(s => s.Number);
        MaxRow = Seats.Max(s => s.Row);
    }

    public void Run()
    {
        Running = true;
        while (Running)
        {
            Console.Clear();
            PrintSeatSelectionContent();
            
            RedrawSeatGrid();

            ConsoleKeyInfo pressedKey = Console.ReadKey();
            HandledEvent(pressedKey);
        }
    }

    public void RedrawSeatGrid()
    {

        PrintSeatNumbers();
        PrintSeats();
    }

    private void HandledEvent(ConsoleKeyInfo pressedKey)
    {
        if (pressedKey.Key == ConsoleKey.Enter)
        {
            ReservationLogic.AddOrRemoveSeat(SelectedSeat);
            return;
        }

        if (pressedKey.Key == ConsoleKey.Backspace)
        {
            Running = false;
            return;
        }

        SelectedSeat = Move(pressedKey);
    }

    private void PrintSeats()
    {
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
                Console.Write($"{seat.Row:D2} ║     ");
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
                Console.BackgroundColor = ConsoleColor.Magenta;
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

            PrintWalkingPath(seat);
        }
    }

    private void PrintSeatSelectionContent()
    {
        Console.WriteLine("Selected movie:");
        Console.WriteLine(SelectedMovie);
        Console.WriteLine();

        Console.WriteLine("╔═════╗");
        Console.WriteLine("║  C  ║  Press c to confirm seats and proceed to payment");
        Console.WriteLine("╚═════╝");

        Console.WriteLine("╔═════════════╗");
        Console.WriteLine("║  Backspace  ║  Press Backspace to go back");
        Console.WriteLine("╚═════════════╝");

        Console.WriteLine("╔═════════╗");
        Console.WriteLine("║  Enter  ║  Press Enter to select a seat");
        Console.WriteLine("╚═════════╝");

        Console.WriteLine();

        Console.WriteLine("[ ] - Available (free)");
        Console.WriteLine("[S] - Selected (your choice)");
        Console.WriteLine("[X] - Reserved (already taken)");

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("[ ] - Normal seat");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ ] - Love seat");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ ] - VIP seat\n");

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

    private void PrintSeatNumbers()
    {
        string numbers = "         ";
        string line = "   ╔══════";
        for (int i = 1; i <= MaxNumber; i++)
        {
            string adding = "";
            if ((SelectedShowtime.AuditoriumId == 2 && (i == 6 || i == 12)) ||
                (SelectedShowtime.AuditoriumId == 3 && (i == 11 || i == 19)))
            {
                adding = "   ";
                line += "══";
            }

            numbers += $"{i:D3} {adding}";
            line += "════";
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(numbers);
        Console.WriteLine(line);
        Console.WriteLine("   ║       ");
        Console.WriteLine("   ║       ");
    }

    private void PrintWalkingPath(Seat seat)
    {
        if (((seat.Number == 6 || seat.Number == 12) && seat.AuditoriumId == 2) ||
            (seat.AuditoriumId == 3 && (seat.Number == 11 || seat.Number == 19)))
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("  ");

            Console.ResetColor();
            Console.Write(" ");
        }

        if (seat.AuditoriumId == 3)
        {
            if (seat.Row == 10 || seat.Row == 15)
            {
                IEnumerable<Seat> targetedSeats = Seats.Where(s => s.Row == seat.Row);
                int maxNum = targetedSeats.Max(s => s.Number);

                if (seat.Number == maxNum)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("\n   ║     ");
                    Console.ResetColor();

                    string line = "     ";
                    for (int i = 0; i < maxNum; i++)
                    {
                        line += "    ";
                    }

                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(line);
                    Console.ResetColor();
                }
            }
        }
    }
}