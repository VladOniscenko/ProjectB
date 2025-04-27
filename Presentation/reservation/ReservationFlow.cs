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
    private readonly IReservationService _reservationService;
    private readonly Movie _movie;
    private Showtime _showtime { get; set; } = null;
    private IEnumerable<Seat>? _seats { get; set; } = null;
    private ReservationState _currentState = ReservationState.Showtime;
    private string? _paymentMethod = null;
    private bool Running = false;
    private Auditorium? _auditorium { get; set; } = null;

    public ReservationFlow(IServiceProvider services, Movie movie)
    {
        _services = services;
        _auditoriumService = services.GetRequiredService<IAuditoriumService>();
        _seatService = services.GetRequiredService<ISeatService>();
        _reservationService = services.GetRequiredService<IReservationService>();
        _movie = movie;

        _currentState = ReservationState.Showtime;
    }

    enum ReservationState
    {
        Showtime = 0,
        Seats = 1,
        Tickets = 2,
        Authenticate = 3,
        PaymentMethod = 4,
        Pay = 5,
        CreateReservation = 6,
        Completed = 7
    }

    public void Run()
    {
        Running = true;
        while (Running)
        {
            string nextButtonName = _currentState switch
            {
                ReservationState.Showtime => "Select Showtime",
                ReservationState.Seats => "Select Seats",
                ReservationState.Tickets => "Select Tickets",
                ReservationState.Authenticate => "Check out",
                ReservationState.PaymentMethod => "Select Payment method",
                _ => "Next step"
            };
            
            var options = new Dictionary<string, string>()
            {
                { "NX", nextButtonName }
            };
            
            options.Add("SB", "Previous step");

            if (_currentState != ReservationState.CreateReservation && _currentState != ReservationState.PaymentMethod && _currentState != ReservationState.Completed && _currentState != ReservationState.Pay)
            {
                switch (Menu.SelectMenu(GetReservationInfo() + "\n\nSelect an option", options))
                {
                    case "SB":
                        switch (_currentState)
                        {
                            case ReservationState.Seats:
                                _currentState = ReservationState.Showtime;
                                _showtime = null;
                                _auditorium = null;
                                break;
                            case ReservationState.Tickets:
                                _currentState = ReservationState.Seats;
                                _seats = null;
                                break;
                            case ReservationState.Authenticate:
                                _currentState = ReservationState.Tickets;
                                foreach (var seat in _seats)
                                {
                                    seat.TicketType = null;
                                }
                                break;
                            case ReservationState.PaymentMethod:
                                _currentState = ReservationState.Tickets;
                                break;
                        }

                        break;

                    case "CR":
                        Running = false;
                        break;
                }
            }

            if (Running)
            {
                // Checks what is the current state and handles it
                // if the state is showtime it opens showtime selection etc.
                HandleReservationStep();
            }

        }



        // 4. logged in? go to step 6

        // 5. Ask to login or to register

        // 6. confirmation (total price seats etc.)

        // 7. pay
    }


    private string SelectedShowtimeInfo()
    {
        if (_showtime == null) return "";


        var sb = new StringBuilder();
        int contentWidth = 72; // 76 breedte - 2║ en 2 spaties

        sb.AppendLine();
        sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                             Selected Showtime                            ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine("║  Date        ║  Start    ║  End      ║  Auditorium                       ║");
        sb.AppendLine("╠══════════════╬═══════════╬═══════════╬═══════════════════════════════════╣");

        string date = _showtime.StartTime.ToString("dd-MM-yyyy");
        string start = _showtime.StartTime.ToString("HH:mm");
        string end = _showtime.EndTime.ToString("HH:mm");
        string auditorium = (_auditorium?.Name ?? "N/A").PadRight(33).Substring(0, 31);

        sb.AppendLine($"║  {date,-10}  ║  {start,-7}  ║  {end,-7}  ║  {auditorium}  ║");
        sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }


    private string SelectedMovieInfo()
    {
        var sb = new StringBuilder();
        int contentWidth = 70;

        sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                               Selected Movie                             ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");

        sb.AppendLine($"║  {TruncateAndPad($"Title: {_movie.Title}", contentWidth)}  ║");
        sb.AppendLine($"║  {TruncateAndPad($"Runtime: {_movie.Runtime} minutes", contentWidth)}  ║");
        sb.AppendLine($"║  {TruncateAndPad($"Actors: {_movie.Actors}", contentWidth)}  ║");
        sb.AppendLine($"║  {TruncateAndPad($"Rating: {_movie.Rating}/10", contentWidth)}  ║");
        sb.AppendLine($"║  {TruncateAndPad($"Genre: {_movie.Genre}", contentWidth)}  ║");

        if (_movie.AgeRestriction > 0)
            sb.AppendLine($"║  {TruncateAndPad($"Age restriction: {_movie.AgeRestriction}", contentWidth)}  ║");

        sb.AppendLine($"║  {TruncateAndPad($"Release date: {_movie.ReleaseDate:yyyy-MM-dd}", contentWidth)}  ║");
        sb.AppendLine($"║  {TruncateAndPad($"Country: {_movie.Country}", contentWidth)}  ║");

        sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }

    private string SelectedSeatsInfo()
    {
        if (_seats == null || !_seats.Any()) return "";

        decimal totalPrice = _seatService.GetTotalPrice(_seats);
        var sb = new StringBuilder();

        sb.AppendLine();
        sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                               Selected Seats                             ║");
        sb.AppendLine("╠══════════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine("║  Row    ║  Seat   ║  Seat Type     ║  Ticket Type    ║  Price            ║");
        sb.AppendLine("╠═════════╬═════════╬════════════════╬═════════════════╬═══════════════════╣");

        foreach (var seat in _seats)
        {
            string row = TruncateAndPad(seat.Row.ToString(), 6);
            string number = TruncateAndPad(seat.Number.ToString(), 6);
            string seatType = TruncateAndPad(seat.Type ?? "N/A", 13);
            string ticketType = TruncateAndPad(seat.TicketType ?? "N/A", 15);

            string price = $"€{_seatService.CalculateSeatPrice(seat):0.00}".PadRight(15);
            if (_seatService.CalculateSeatPrice(seat) <= 0)
            {
                price = "N/A".PadRight(15);
            }
            
            sb.AppendLine($"║  {row} ║  {number} ║  {seatType} ║ { ticketType} ║  {price}  ║");
        }

        sb.AppendLine("╚═════════╩═════════╩════════════════╩═════════════════╩═══════════════════╝");
        return sb.ToString();
    }

    private string TotalPriceInfo()
    {
        if (_seats == null || !_seats.Any() || _currentState <=
            
            
            ReservationState.Tickets)
            return string.Empty;

        decimal totalPrice = _seatService.GetTotalPrice(_seats);
        string totalLine = $"║  Total price: €{totalPrice:0.00}".PadRight(75) + "║";

        var sb = new StringBuilder();
        sb.AppendLine(totalLine);
        sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }



    private string GetReservationInfo()
    {
        var sb = new StringBuilder();

        sb.Append(SelectedMovieInfo());
        sb.Append(SelectedShowtimeInfo());
        sb.Append(SelectedSeatsInfo());
        sb.Append(TotalPriceInfo());

        return sb.ToString();
    }

    public void HandleReservationStep()
    {
        switch (_currentState)
        {
            case ReservationState.Showtime:
                // 1. Handle showtime selection
                _showtime = new SelectShowtime(_services, _movie).Run();
                if (_showtime != null)
                {
                    _currentState = ReservationState.Seats;
                    _auditorium = _auditoriumService.Find(_showtime.AuditoriumId);
                    break;
                }
                
                Running = false;
                break;

            case ReservationState.Seats:
                // 2. select seats
                _seats = new SeatSelection(_services, _movie, _showtime).Run();
                if (_seats != null && _seats.Count() > 0)
                {
                    _currentState = ReservationState.Tickets;
                }

                break;

            case ReservationState.Tickets:
                // 2. select tickets type (childrent, adults, seniors)
                IEnumerable<Seat> seats = new TicketSelection(_services, _seats).Run();
                if (seats != null && seats.Count() > 0)
                {
                    _currentState = ReservationState.Authenticate;
                    _seats = seats;
                }

                break;
            case ReservationState.Authenticate:

                if (Program.CurrentUser != null)
                {
                    _currentState = ReservationState.PaymentMethod;
                    break;
                }

                new Authenticate(_services).Run();
                if (Program.CurrentUser != null)
                {
                    _currentState = ReservationState.PaymentMethod;
                }
                break;
            case ReservationState.PaymentMethod:
                _paymentMethod = new SelectPaymentMethod(_services).Run();
                if(_paymentMethod != null)
                {
                    _currentState = ReservationState.Pay;
                }
                break;
            case ReservationState.Pay:
                _currentState = ReservationState.CreateReservation;
                break;
            case ReservationState.CreateReservation:
                try
                {
                    var reservationError = _reservationService.CreateReservation(_showtime.Id, _seats, _paymentMethod, Program.CurrentUser.Id);
                    if (reservationError != null && reservationError.ErrorCode != "SUCCESS")
                    {
                        // Handle error and show appropriate message
                        ConsoleMethods.Error(reservationError.Message);
                        _currentState = ReservationState.Seats;
                        break;
                    }
                    
                    _currentState = ReservationState.Completed;
                }
                catch (Exception ex)
                {
                    // General exception fallback
                    ConsoleMethods.Error("An unexpected error occurred. Please try again.");
                }
                break;
            case ReservationState.Completed:
                // 7. Show success message
                Console.Clear();
                ConsoleMethods.Success("Reservation completed!");
                Running = false;
                break;
        }
    }


    private string TruncateAndPad(string text, int maxLength)
    {
        if (text == null) return "".PadRight(maxLength);
        if (text.Length > maxLength)
            return text.Substring(0, maxLength - 3) + "...";
        return text.PadRight(maxLength);
    }
}