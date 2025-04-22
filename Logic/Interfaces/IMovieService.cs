using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IMovieService
{
    bool CreateMovie(Movie movie);
    IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7);
    Movie? Find(int id);
    IEnumerable<Movie> All();
}