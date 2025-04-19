using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class MovieLogic : IMovieService
{
    private readonly MovieRepository _movieRepository;

    public MovieLogic(MovieRepository movieRepo)
    {
        _movieRepository = movieRepo;
    }
    
    public bool CreateMovie(Movie movie)
    {
        _movieRepository.AddMovie(movie);
        return true;
    }

    public IEnumerable<Movie> GetMoviesWithShowtimeInNextDays(int days = 7)
    {
        return _movieRepository.GetMoviesWithShowtimeInNextDays(days);
    }
    
    public Movie? Find(int id)
    {
        return _movieRepository.Find(id);
    }

    public IEnumerable<Movie> All()
    {
        return _movieRepository.GetAllMovies();
    }
}