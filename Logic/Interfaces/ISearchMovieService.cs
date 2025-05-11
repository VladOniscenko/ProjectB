using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface ISearchMovieService
{
    public List<Movie> GetSearchedMovies(string title);
}