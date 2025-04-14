using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public class SeatLogic
{
    public static IEnumerable<Seat> GetSeatsByAuditorium(int id)
    {
        SeatRepository seatRepository = new();
        return seatRepository.GetSeatsByAuditorium(id);
    }
}
