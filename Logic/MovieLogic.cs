using System.Data;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using System.Globalization;


namespace ProjectB.Logic;

public class MovieLogic : IMovieService
{
    private readonly MovieRepository _movieRepository;

    public MovieLogic(MovieRepository movieRepo)
    {
        _movieRepository = movieRepo;
    }
    
    public bool CreateMovie(string title, string description, int runtime, string actors, 
        double rating, string genre, int ageRestriction, DateTime releaseDate, string country)
    {
        var movie = new Movie
        {
            Title = title,
            Description = description,
            Runtime = runtime,
            Actors = actors,
            Rating = rating,
            Genre = genre,
            AgeRestriction = ageRestriction,
            ReleaseDate = releaseDate,
            Country = country
        };
    
        _movieRepository.AddMovie(movie);
        return true;
    }

    public IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7, int limit = 50)
    {
        return _movieRepository.GetMoviesWithShowtimeInNextDays(days, limit);
    }
    
    public Movie? Find(int id)
    {
        return _movieRepository.Find(id);
    }

    public IEnumerable<Movie> All()
    {
        return _movieRepository.GetAllMovies();
    }

    public List<Movie> GetPromotedMovies()
    {
        return _movieRepository.GetBestAndNewestMovies(3);
    }
    
    public static bool ValidateInput<T>(int min = 0, int max = 100, string? input = null)
    {
        try
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
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    if (value >= min && value <= max)
                    {
                        return true;
                    }
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(input, out DateTime date) && date.Year >= min && date.Year <= max)
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
    
    public IEnumerable<Movie> GetMoviesByTitle(string title, int limit = 10)
    {
        return _movieRepository.GetMoviesByTitle(title, limit);
    }
}