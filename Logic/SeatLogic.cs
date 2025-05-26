using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class SeatLogic : ISeatService
{
    private SeatRepository _seatRepository;

    public SeatLogic(SeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public IEnumerable<Seat> GetSeatsByShowtime(int showtimeId)
    {
        SeatRepository seatRepository = new();
        return seatRepository.GetSeatsByShowtime(showtimeId);
    }

    public Dictionary<string, string> GetTicketOptionsForSeat(Seat seat)
    {
        var ticketTypes = new List<string> { "adult", "child", "senior" };
        var adjustedOptions = new Dictionary<string, string>();
        foreach (var ticketType in ticketTypes)
        {
            decimal finalPrice = CalculateSeatPrice(seat, ticketType);
            string description = ticketType switch
            {
                "adult" => "Adult ticket (18+)",
                "child" => "Child ticket (<18)",
                "senior" => "Senior ticket (65+)",
                _ => ticketType
            };

            adjustedOptions.Add(ticketType, $"{description} €{finalPrice:F2}");
        }

        return adjustedOptions;
    }

    public decimal CalculateSeatPrice(Seat seat)
    {
        return CalculateSeatPrice(seat, seat.TicketType);
    }

    public decimal CalculateSeatPrice(Seat seat, string? ticketType)
    {
        Dictionary<string, decimal> baseTicketPrices = new()
        {
            { "adult", 15.00m },
            { "child", 10.00m },
            { "senior", 10.00m }
        };

        Dictionary<string, decimal> seatTypeAdjustments = new()
        {
            { "normal", 0.00m },
            { "premium", 3.00m },
            { "vip", 5.00m }
        };

        string? safeTicketType = ticketType?.ToLower() ?? null;
        string? safeSeatType = seat.Type?.ToLower() ?? null;

        if (safeTicketType == null || safeSeatType == null)
        {
            return 0.00m;
        }

        decimal basePrice = baseTicketPrices.GetValueOrDefault(safeTicketType, 0.00m);
        decimal adjustment = seatTypeAdjustments.GetValueOrDefault(safeSeatType, 0.00m);

        return basePrice + adjustment;
    }

    public decimal GetTotalPrice(IEnumerable<Seat> seats)
    {
        decimal total = 0.00m;

        foreach (var seat in seats)
        {
            total += CalculateSeatPrice(seat);
        }

        return total;
    }
}