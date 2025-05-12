using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISearchMovieService
{
    public List<Movie> GetSearchedMoviesByTitle(string title);
    public List<Movie> GetSearchedMoviesByTitle(string title, int amount);
    public List<Movie> GetSearchedMoviesByGenre(string genre);
    public List<Movie> GetSearchedMovieByActor(string actor);
    public List<Movie> GetSearchedMovieByTitleAndGenre(string title, string genre);
    public List<Movie> GetSearchedMovieByTitleAndActor(string title, string actor);
    public List<Movie> GetSearchedMovieByGenreAndActor(string genre, string actor);

    public List<Movie> GetSearchedMovieByTitleGenreAndActor(string title, string genre, string actor);
    public List<Movie> FindSpeceficMovieList(string title, string genre, string actor);
    public bool DoesGenreExist(string genre);

}