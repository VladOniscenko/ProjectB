using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISeatReservationService
{
    IEnumerable<SeatReservation> GetReservedSeatsByShowtimeId(int showtimeId);
    void Create(SeatReservation seatReservation);
    bool IsSeatTaken(int showtimeId, int seatId);
}