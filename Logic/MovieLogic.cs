using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public static class MovieLogic
{
    public static bool CreateMovie(Movie movie)
    {
        MovieRepository movieRepository = new MovieRepository();
        movieRepository.AddMovie(movie);
        return true;
    }

    public static IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7)
    {
        MovieRepository movieRepository = new MovieRepository();
        return movieRepository.GetMoviesWithShowtimeInNextDays(days);
    }
}