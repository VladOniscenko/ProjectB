using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public class SeatLogic
{
    public static IEnumerable<Seat> GetSeatsByShowtime(int showtimeId)
    {
        SeatRepository seatRepository = new();
        return seatRepository.GetSeatsByShowtime(showtimeId);
    }
}
