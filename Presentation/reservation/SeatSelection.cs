using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SeatSelection
{
    private readonly Showtime? _showtime;
    private readonly Movie? _movie;
    private readonly Auditorium? _selectedAuditorium;
    private readonly IEnumerable<Seat>? _seats;
    private readonly List<Seat>? _selectedSeats;
    
    private readonly int _maxNumber;
    private readonly int _maxRow;

    private readonly IServiceProvider _services;
    private readonly ISeatService _seatService;
    private Seat? _selectedSeat;
    

    public SeatSelection(IServiceProvider services, Movie movie, Showtime showtime)
    {
        _services = services;
        _showtime = showtime;
        _movie = movie;
        
        var seatService = _services.GetRequiredService<ISeatService>();
        var auditoriumService = _services.GetRequiredService<IAuditoriumService>();
        _seatService = _services.GetRequiredService<ISeatService>();

        _seats = seatService.GetSeatsByShowtime(_showtime.Id);
        _selectedAuditorium = auditoriumService.Find(_showtime.AuditoriumId);
        if (_movie == null || _seats == null || _seats.Count() <= 0)
        {
            ConsoleMethods.Error("Something went wrong! Get in touch with our Customer service!");
            return;
        }

        // get first active seat
        _selectedSeat = _seats.FirstOrDefault(s => s.Active == 1);

        _maxNumber = _seats.Max(s => s.Number);
        _maxRow = _seats.Max(s => s.Row);
        _selectedSeats = new List<Seat>();
    }

    public List<Seat>? Run()
    {
        while (true)
        {
            Console.Clear();
            PrintSeatSelectionContent();
            
            RedrawSeatGrid();

            ConsoleKeyInfo pressedKey = Console.ReadKey();
            // ConsoleMethods.Success($"{pressedKey.Key}");
            if (pressedKey.Key == ConsoleKey.Enter)
            {
                AddOrRemoveSeat(_selectedSeat);
                continue;
            }

            if (pressedKey.Key == ConsoleKey.Backspace)
            {
                return null;
            }

            if (pressedKey.Key == ConsoleKey.C)
            {
                Console.Clear();
                return _selectedSeats;
            }

            _selectedSeat = Move(pressedKey);
        }
    }

    private void RedrawSeatGrid()
    {

        PrintSeatNumbers();
        PrintSeats();
        PrintAuditoriumScreen();
    }

    private void PrintAuditoriumScreen()
    {
        int len = ((_maxNumber * 4) - 1) + ((_selectedAuditorium.Id == 3 || _selectedAuditorium.Id == 2) ? 6 : 0);
        string screen = "SCREEN";
        string padded = "SCREEN".PadLeft((screen.Length + len) / 2).PadRight(len);
        
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("         ╔" + ( new string('═', len ) ) + "╗");
        Console.WriteLine($"         ║{padded}║");
        Console.WriteLine("         ╚"  + ( new string('═', len ) ) +  "╝");
    }

    private void PrintSeats()
    {
        // display all seats in the console
        int? currentRow = null;
        foreach (var seat in _seats)
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
                "premium" => ConsoleColor.Yellow,
                "vip" => ConsoleColor.Red,
                "normal" => ConsoleColor.Blue,
                _ => null
            };

            // highlight the seat that is currently selected
            if (_selectedSeat.Id == seat.Id)
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
        Console.WriteLine("╔═════════════╗");
        Console.WriteLine("║  Backspace  ║  Press Backspace to go to previous step");
        Console.WriteLine("╚═════════════╝");
        
        Console.WriteLine("╔═════╗");
        Console.WriteLine("║  C  ║  Press c to confirm seats and proceed to payment");
        Console.WriteLine("╚═════╝");

        Console.WriteLine("╔═════════╗");
        Console.WriteLine("║  Enter  ║  Press Enter to select a seat");
        Console.WriteLine("╚═════════╝");

        Console.WriteLine();

        Console.WriteLine("[ ] - Available (free)");
        Console.WriteLine("[S] - Selected (your choice)");
        Console.WriteLine("[X] - Reserved (already taken)");

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[ ] - Normal seat (€{_seatService.CalculateSeatPrice(new Seat(){Type = "normal"}, "adult")})");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[ ] - Premium seat (€{_seatService.CalculateSeatPrice(new Seat(){Type = "premium"}, "adult")})");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ ] - VIP seat (€{_seatService.CalculateSeatPrice(new Seat(){Type = "vip"}, "adult")})");
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
        var nextSeat = _seats.FirstOrDefault(
            s =>
                s.Row == _selectedSeat.Row + row &&
                s.Number == _selectedSeat.Number + number &&
                s.Active == 1
        );

        // if seat not found return current seat. otherwise return found seat
        return nextSeat == null ? _selectedSeat : nextSeat;
    }

    private void PrintSeatNumbers()
    {
        string numbers = "         ";
        string line = "   ╔══════";
        for (int i = 1; i <= _maxNumber; i++)
        {
            string adding = "";
            if ((_showtime.AuditoriumId == 2 && (i == 6 || i == 12)) ||
                (_showtime.AuditoriumId == 3 && (i == 11 || i == 19)))
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
                IEnumerable<Seat> targetedSeats = _seats.Where(s => s.Row == seat.Row);
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
    
    private void AddSeat(Seat seat)
    {
        if (seat.Taken == 0)
        {
            seat.Selected = true;
            _selectedSeats.Add(seat);
        }
    }
    
    private void RemoveSeat(Seat seat)
    {
        if (_selectedSeats.Contains(seat))
        {
            seat.Selected = false;
            _selectedSeats.Remove(seat);
        }
    }
    
    public void AddOrRemoveSeat(Seat seat)
    {
        if (seat.Selected)
        {
            RemoveSeat(seat);
            return;
        }
        AddSeat(seat);
    }
}