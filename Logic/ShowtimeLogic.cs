using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public class ShowtimeLogic
{
    public static Showtime? Find(int id)
    {
        ShowtimeRepository showtimeRepository = new();
        return showtimeRepository.Find(id);
    }

    public static IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 5)
    {
        ShowtimeRepository showtimeRepository = new();
        return showtimeRepository.GetShowtimesByMovieId(movieId, limit);
    }
}