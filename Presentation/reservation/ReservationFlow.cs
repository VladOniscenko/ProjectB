using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ReservationFlow
{
    private ReservationLogic Reservation;
    private Showtime SelectedShowtime;
    
    private readonly IServiceProvider _services;
    private readonly IAuditoriumService _auditoriumService;
    private readonly ISeatService _seatService;
    private readonly Movie _movie;
    private Showtime _showtime { get; set; } = null;
    private IEnumerable<Seat>? _seats { get; set; } = null;
    private ReservationState _currentState = ReservationState.Showtime;
    private bool Running = false;
    private Auditorium? _auditorium { get; set; } = null;
    
    public ReservationFlow(IServiceProvider services, Movie movie)
    {
        _services = services;
        _auditoriumService = services.GetRequiredService<IAuditoriumService>();
        _seatService = services.GetRequiredService<ISeatService>();
        _movie = movie;
    }
    
    enum ReservationState
    {
        Showtime = 0,
        Seats = 1,
        Tickets = 2,
        Confirmation = 3,
        Payment = 4,
        Success = 5,
        Cancelled = 6,
        Error = 7,
    }

    public void Run()
    {
        Running = true;
        while (Running)
        {
            // Checks what is the current state and handles it
            // if the state is showtime it opens showtime selection etc.
            HandleReservationStep();
            
            var options = new Dictionary<string, string>()
            {
                {"NX", "Go further"}
            };
            
            if (_currentState != ReservationState.Showtime)
            {
                options.Add("SB", "Step back");
            }

            options.Add("CR", "Cancel reservation and go back");
            switch (Menu.SelectMenu(GetReservationInfo() + "\n\nSelect an option", options))
            {
                case "SB":
                    if (_currentState == ReservationState.Seats)
                    {
                        _currentState = ReservationState.Showtime;
                        _showtime = null;
                        _auditorium = null;
                    }

                    if (_currentState == ReservationState.Tickets)
                    {
                        _currentState = ReservationState.Seats;
                        _seats = null;
                    }
                    
                    if (_currentState == ReservationState.Confirmation)
                    {
                        _currentState = ReservationState.Tickets;
                        foreach (var seat in _seats)
                        {
                            seat.TicketType = null;
                        }
                    }
                    
                    break;
                
                case "CR":
                    Running = false;
                    break;
            }
        }

        
        
        

        


        // 3. select tickets type (childrent, adults, seniors)

        // 4. logged in? go to step 6

        // 5. Ask to login or to register

        // 6. confirmation (total price seats etc.)

        // 7. pay
    }

    private string GetReservationInfo()
    {
        var sb = new StringBuilder();
        string separator = new string('=', Console.WindowWidth);
    
        sb.AppendLine(separator);
        sb.AppendLine("Selected Movie");
        sb.AppendLine(separator);
    
        sb.AppendLine($"Title: {_movie.Title}");
        sb.AppendLine($"Runtime: {_movie.Runtime} minutes");
        sb.AppendLine($"Actors: {_movie.Actors}");
        sb.AppendLine($"Rating: {_movie.Rating}/10");
        sb.AppendLine($"Genre: {_movie.Genre}");
        sb.AppendLine($"Age restriction: {_movie.AgeRestriction}");
        sb.AppendLine($"Release date: {_movie.ReleaseDate}");
        sb.AppendLine($"Country: {_movie.Country}");

        if (_showtime != null)
        {
            sb.AppendLine();
            sb.AppendLine(separator);
            sb.AppendLine("Selected Showtime");
            sb.AppendLine(separator);
            sb.AppendLine($"Start: {_showtime.StartTime}");
            
            
            
            sb.AppendLine($"End: {_showtime.EndTime}");
            sb.AppendLine($"Auditorium: {_auditorium.Name}");
        }

        if (_seats != null && _seats.Count() > 0)
        {
            sb.AppendLine();
            sb.AppendLine(separator);
            sb.AppendLine("Selected Seats");
            sb.AppendLine(separator);

            
            
            
            decimal totalPrice = _seatService.GetTotalPrice(_seats);

            sb.AppendLine("╔════════╦══════════╦════════════╦════════════════════════╗");
            sb.AppendLine("║ Row    ║ Seat     ║ Seat Type  ║ Ticket Type  ║  Price  ║");
            sb.AppendLine("╠════════╬══════════╬════════════╬══════════════╬═════════╣");

            foreach (var seat in _seats)
            {
                string row = seat.Row.ToString().PadRight(6);
                string number = seat.Number.ToString().PadRight(8);
                string seatType = seat.Type?.PadRight(10) ?? "N/A".PadRight(10);
                string ticketType = seat.TicketType?.PadRight(12) ?? "N/A".PadRight(12);
                decimal price = _seatService.CalculateSeatPrice(seat);

                sb.AppendLine($"║ {row} ║ {number} ║ {seatType} ║ {ticketType} ║ ${price,5:F2}  ║");
            }

            sb.AppendLine("╚════════╩══════════╩════════════╩══════════════╩═════════╝");
            sb.AppendLine($"Total price: ${totalPrice:F2}");
            
        }

        return sb.ToString();
    }

    public void HandleReservationStep()
    {
        switch (_currentState)
        {
            case ReservationState.Showtime:
                // 1. Handle showtime selection
                SelectShowtime selectShowtime = new SelectShowtime(_services, _movie);
                _showtime = selectShowtime.Run();
                if (_showtime != null)
                {
                    _currentState = ReservationState.Seats;
                    _auditorium = _auditoriumService.Find(_showtime.AuditoriumId);
                }
                    
                break;
            
            case ReservationState.Seats:
                // 2. select seats
                SeatSelection? seatSelection = new SeatSelection(_services, _movie, _showtime);
                _seats = seatSelection.Run();
                if (_seats != null && _seats.Count() > 0)
                {
                    _currentState = ReservationState.Tickets;
                }
                break;
            
            case ReservationState.Tickets:
                // 2. select tickets type (childrent, adults, seniors)
                TicketSelection ticketSelection = new(_services, _seats);
                IEnumerable<Seat> seats = ticketSelection.Run();
                if (seats != null && seats.Count() > 0)
                {
                    _currentState = ReservationState.Confirmation;
                    _seats = seats;
                }
                break;
            case ReservationState.Confirmation:
                ConsoleMethods.Error("Not implemented yet");
                break;
        }
    }
}