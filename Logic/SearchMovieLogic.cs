using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

public class SearchMovieLogic : ISearchMovieService{

    private readonly MovieRepository _movieRepository;

    public SearchMovieLogic(MovieRepository movieRepository){
        _movieRepository = movieRepository;
    }

    public List<Movie> GetSearchedMovies(string title){
        return _movieRepository.GetMoviesByTitle(title);
    }

}