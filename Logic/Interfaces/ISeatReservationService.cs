using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISeatReservationService
{
    IEnumerable<SeatReservation> GetReservedSeatsByShowtimeId(int showtimeId);
}