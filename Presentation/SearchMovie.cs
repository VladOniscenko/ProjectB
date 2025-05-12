using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;

public class SearchMovie{

    private readonly IServiceProvider _services;
    private readonly ISearchMovieService _searchMovieService;

    public SearchMovie(IServiceProvider services){
        _services = services;
        _searchMovieService = _services.GetRequiredService<ISearchMovieService>();
    }


    public void SearchForMovies(){

        while(true){
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.Write("Movie name: ");
        Console.SetCursorPosition(0, 1);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', 50));
        Console.ResetColor();

        Console.SetCursorPosition(0, 3);
        Console.Write("Genre: ");
        Console.SetCursorPosition(0, 4);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', 20));
        Console.ResetColor();

        
        // Console.SetCursorPosition(28, 3);
        // Console.Write("Release date (in dd-mm-yy): ");
        // Console.SetCursorPosition(28, 4);
        // Console.BackgroundColor = ConsoleColor.White;
        // Console.ForegroundColor = ConsoleColor.Black;
        // Console.Write(new string(' ', 35));
        // Console.ResetColor();


        Console.SetCursorPosition(0, 6);
        Console.Write("Actor's/Actress' name: ");
        Console.SetCursorPosition(0, 7);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', 30));

        Console.SetCursorPosition(0, 1);
        string movieName = BaseUI.ReadInputBox(0, 50, false, 1, null);
        Console.SetCursorPosition(0,4);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        string genre = BaseUI.ReadInputBox(0, 20, false, 4, null);
        Console.SetCursorPosition(0,7);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        string actor = BaseUI.ReadInputBox(0, 30, false, 7, null);
        Console.ResetColor();
        Console.Clear();
        // Console.WriteLine($"Movies found for'{movieName}'\n=============================");

        // foreach(Movie movie in _searchMovieService.GetSearchedMovies(movieName, 30)){
        //     Console.WriteLine(movie.Title);
        // }

        List<Movie> foundMovies;


        //vm moet dit in logic layer
        if(movieName != "" && genre != "" && actor ==""){
            foundMovies = _searchMovieService.GetSearchedMovieByTitleAndGenre(movieName, genre);
        }
        else if(movieName != "" && genre == "" && actor !=""){
            foundMovies = _searchMovieService.GetSearchedMovieByTitleAndActor(movieName, actor);
        }
        else if(movieName == "" && genre != "" && actor !=""){
            foundMovies = _searchMovieService.GetSearchedMovieByGenreAndActor(genre, actor);
        }
        else if(movieName != "" && genre != "" && actor !=""){
            foundMovies = _searchMovieService.GetSearchedMovieByTitleGenreAndActor(movieName, genre, actor);
        }
        else{
            foundMovies = _searchMovieService.GetSearchedMoviesByTitle(movieName, 30);
        }
        

        MovieList movieList = new(_services);
        movieList.Run(foundMovies);

        if(!movieList.Running){
            break;
        }
        }
    }
}