using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IShowtimeService
{
    Showtime? Find(int id);
    IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 10);
    bool IsMovieIDValid(string movie);
    bool CheckIfDataCorrect(string movie, int auditorium);
    void CreateShowtime(Showtime showtime);
    int ShowMenuMovies(string title, List<Movie> options);
    int ShowMenuAuditoriums(string title, List<Auditorium> options);
}
