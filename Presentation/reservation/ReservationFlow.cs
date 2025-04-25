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
                { "NX", "Go further" }
            };

            if (_currentState != ReservationState.Showtime)
            {
                options.Add("SB", "Step back");
            }

            options.Add("CR", "<= Return to movie list");
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
        if (_seats == null || !_seats.Any()) return string.Empty;

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



    private string GetReservationInfo()
    {
        var sb = new StringBuilder();

        sb.Append(SelectedMovieInfo());
        sb.Append(SelectedShowtimeInfo());
        sb.Append(SelectedSeatsInfo());

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


    private string TruncateAndPad(string text, int maxLength)
    {
        if (text == null) return "".PadRight(maxLength);
        if (text.Length > maxLength)
            return text.Substring(0, maxLength - 3) + "...";
        return text.PadRight(maxLength);
    }
}