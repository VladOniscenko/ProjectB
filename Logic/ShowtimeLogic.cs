using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class ShowtimeLogic : IShowtimeService
{
    private readonly ShowtimeRepository _showtimeRepository;
    public ShowtimeLogic(ShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }
    
    public Showtime? Find(int id)
    {
        ShowtimeRepository showtimeRepository = new();
        return showtimeRepository.Find(id);
    }

    public IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 5)
    {
        ShowtimeRepository showtimeRepository = new();
        return showtimeRepository.GetShowtimesByMovieId(movieId, limit);
    }
}