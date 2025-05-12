using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

public class SearchMovieLogic : ISearchMovieService{

    private readonly MovieRepository _movieRepository;

    public SearchMovieLogic(MovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public List<Movie> GetSearchedMoviesByTitle(string title)
    {
        return _movieRepository.GetMoviesByTitle(title);
    }

    public List<Movie> GetSearchedMoviesByTitle(string title, int amount)
    {
        return _movieRepository.GetMoviesByTitle(title, amount);
    }

    public List<Movie> GetSearchedMoviesByGenre(string genre)
    {
        return _movieRepository.GetMoviesByGenre(genre);
    }

    public List<Movie> GetSearchedMovieByActor(string actor)
    {
        return _movieRepository.GetMoviesByActor(actor, 30);
    }

    public List<Movie> GetSearchedMovieByTitleAndGenre(string title, string genre)
    {
        return _movieRepository.GetMoviesByTitleAndGenre(title, genre);
    }

    public List<Movie> GetSearchedMovieByTitleAndActor(string title, string actor)
    {
        return _movieRepository.GetMoviesByTitleAndActor(title, actor);
    }

    public List<Movie> GetSearchedMovieByGenreAndActor(string genre, string actor)
    {
        return _movieRepository.GetMoviesByGenreAndActor(genre, actor);
    }

    public List<Movie> GetSearchedMovieByTitleGenreAndActor(string title, string genre, string actor)
    {
        return _movieRepository.GetMoviesByTitleGenreAndActor(title, genre, actor);
    }


    public bool DoesGenreExist(string genre){
        return _movieRepository.GetAllGenres().Contains(genre);
    }



}