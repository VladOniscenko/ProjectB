using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public class SeatReservationLogic
{
    public static IEnumerable<SeatReservation> GetReservedSeatsByShowtimeId(int showtimeId)
    {
        SeatReservationRepository seatReservationRepository = new();
        return seatReservationRepository.GetReservedSeatsByShowtimeId(showtimeId);
    }
}