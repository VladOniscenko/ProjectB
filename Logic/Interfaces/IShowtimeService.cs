using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IShowtimeService
{
    Showtime? Find(int id);
    IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 10);
    void CreateShowtime(Showtime showtime);
    public bool IsMovieIDValid(string movie);
    public bool IsMovieStartTimeValid(string time);
    public DateTime parsedStartTime { get; set; }
}
