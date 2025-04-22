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
}
