using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IShowtimeService
{
    Showtime? Find(int id);
    IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 5);
}
