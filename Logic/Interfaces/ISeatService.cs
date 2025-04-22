using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISeatService
{
    IEnumerable<Seat> GetSeatsByShowtime(int showtimeId);
    Dictionary<string, string> GetTicketOptionsForSeat(Seat seat);
    decimal GetTotalPrice(IEnumerable<Seat> seats);
    decimal CalculateSeatPrice(Seat seat, string? ticketType = null);
    decimal CalculateSeatPrice(Seat seat);
}