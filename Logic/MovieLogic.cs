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

    public static bool ValidateInput<T>(int min = 0, int max = 100, string input = null)
    { 

        if (typeof(T) == typeof(int))
        {
            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return true;
            }
        }
        else if (typeof(T) == typeof(string))
        {
            if (!string.IsNullOrEmpty(input) && input.Length >= min && input.Length <= max)
            {
                return true;
            }
        }
        else if (typeof(T) == typeof(double))
        {
            if (double.TryParse(input, out double value) && value >= min && value <= max )
            {
                return true;
            }
        }
        else if (typeof(T) == typeof(DateTime))
        {
            if (DateTime.TryParse(input, out DateTime _))
            {
                return true;
            }
        }
        return false;
    }


    public static IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7)
    {
        MovieRepository movieRepository = new MovieRepository();
        return movieRepository.GetMoviesWithShowtimeInNextDays(days);
    }
}