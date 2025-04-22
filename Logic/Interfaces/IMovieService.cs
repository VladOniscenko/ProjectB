using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IMovieService
{
    bool CreateMovie(Movie movie);
    IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7, int limit = 50);
    Movie? Find(int id);
    IEnumerable<Movie> All();

    bool ValidateInput<T>(int min = 0, int max = 100, string input = null);
}