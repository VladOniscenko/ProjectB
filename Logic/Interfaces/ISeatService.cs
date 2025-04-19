using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISeatService
{
    IEnumerable<Seat> GetSeatsByShowtime(int showtimeId);
}