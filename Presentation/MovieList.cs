using Microsoft.Extensions.DependencyInjection;
using ProjectB.Models;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;

namespace ProjectB.Presentation;

public class MovieList
{
    private bool Running;
    private const int MaxMoviesPerPage = 5;
    private readonly IServiceProvider _services;

    public MovieList(IServiceProvider services)
    {
        _services = services;
        Running = false;
    }

    // Takes from the Repo and makes a list of movies
    public void Run()
    {
        Console.Clear();
        var _movieLogic = _services.GetRequiredService<IMovieService>();
        IEnumerable<Movie> movies = _movieLogic.GetMoviesWithShowtimeInNextDays(999);

        if (movies.Count() == 0)
        {
            ConsoleMethods.Error("No movies available this week.");
            return;
        }

        int page = 0;

        // calculate total pages based on total movies and max movies to show
        int totalPages = (int)Math.Ceiling((double)movies.Count() / MaxMoviesPerPage);

        Running = true;
        while (Running)
        {
            Console.Clear();
            Console.WriteLine("Movie List");
            Console.WriteLine("===========");

            // get movies and show them in the console
            var moviesToShow = movies.Skip(page * MaxMoviesPerPage).Take(MaxMoviesPerPage).ToList();

            // convert movies to string options to choose from in the menu
            var movieOptions = moviesToShow.ToDictionary(
                m => m.Id.ToString(),
                m => $"{m.Title} | ({m.Runtime} min) | Genres: {m.Genre} | {CalcStars(m.Rating)}"
            );

            // make optiions for the previous and next pagge
            if (page < totalPages - 1)
            {
                movieOptions.Add("N", "Next Page");
            }

            if (page > 0)
            {
                movieOptions.Add("P", "Previous Page");
            }

            movieOptions.Add("M", "Back to Main Menu");
            // show the movies in the menu
            var selectedOption = Menu.SelectMenu($"Select a movie or option [ Page {page + 1}/{totalPages} ]", movieOptions);
            switch (selectedOption)
            {
                case "N":
                    page++;
                    break;
                case "P":
                    page--;
                    break;
                case "M":
                    Running = false;
                    break;
                default:
                    Movie movie = movies.FirstOrDefault(m => m.Id == int.Parse(selectedOption));
                    
                    // show the movie details
                    ShowMovieDetails(movie);
                    ShowPurchaseMenu(movie);
                    continue;
            }

        }
    }

    // Shows the movie in a nice format
    static void ShowMovieDetails(Movie movie)
    {
        Console.Clear();
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("=== MOVIE DETAILS ===");
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"Title           : {movie.Title}");
        Console.WriteLine($"Description     : {movie.Description}");
        Console.WriteLine($"Runtime         : {movie.Runtime} minutes");
        Console.WriteLine($"Actors          : {movie.Actors}");
        Console.WriteLine($"Rating          : {CalcStars(movie.Rating)} ({movie.Rating}/5)");
        Console.WriteLine($"Genre           : {movie.Genre}");
        
        if (movie.AgeRestriction > 0)
        {
            Console.WriteLine($"Age Restriction : {movie.AgeRestriction}");
        }
        
        Console.WriteLine($"Release Date    : {movie.ReleaseDate.ToShortDateString()}");
        Console.WriteLine($"Country         : {movie.Country}");
    }

    /// Shows a menu with options to purchase the selected movie.
    private void ShowPurchaseMenu(Movie movie)
    {
        int startingRow = Console.CursorTop + 2;
        List<string> options = new() { "Check availability", "Back to Movie List" };
        int selected = AddMenuFromStartRow("=== CHOOSE AN OPTION ===", options, startingRow);

        if (selected == 1)
        {
            Console.Clear();
            return;
        }
        
        // start reservation process
        Program.StartReservation(movie);
    }

    /// <summary>
    /// Adds a menu to the console from a specified start row.
    /// </summary>
    /// <param name="title"> Title of the menu </param>
    /// <param name="options"> Options the menu provides </param>
    /// <param name="startRow"> Row from where to start the menu </param>
    /// <param name="rowBuffer"> Used in addition to the startRow param to maniupulate the actual start of writing the menu. </param>
    /// <returns></returns>
    private int AddMenuFromStartRow(string title, List<string> options, int startRow, int rowBuffer = 1)
    {
        int selected = 0;
        ConsoleKey key;

        do
        {   
            // Clear all lines below our starting row. 
            ClearAllLinesDownFrom(startRow - (rowBuffer + 1));

            // Set the cursor in the console to our starting row.
            Console.SetCursorPosition(0, startRow - (rowBuffer + 1));

            // Empty line for spacing.
            Console.WriteLine();

            // Write the menu itself
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine(title);
            Console.WriteLine(new string('-', Console.WindowWidth));

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected == 0) ? options.Count - 1 : selected - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected == options.Count - 1) ? 0 : selected + 1;
                    break;
            }

        // Run while the user has not pressed Enter.
        } while (key != ConsoleKey.Enter);


        // Return the selected option.
        return selected;
    }

    private void ClearAllLinesDownFrom(int startRow)
    {
        int cursorPos = Console.CursorTop;
        int lineAmount = cursorPos - startRow;

        for (int i = 0; i < lineAmount; i++)
        {
            Console.SetCursorPosition(0, startRow + i);
            Console.WriteLine(new string(' ', Console.WindowWidth)); // Clear the line
        }
    }

    private static string CalcStars(double rating)
    {
        string StarString = "";
        int StarsNumber = Convert.ToInt32(rating) / 2;

        switch(StarsNumber)
        {
            case 1:
                StarString = "[*    ]";
                break;
            case 2:
                StarString = "[**   ]";
                break;
            case 3:
                StarString = "[***  ]";
                break;
            case 4:
                StarString = "[**** ]";
                break;
            case 5:
                StarString = "[*****]";
                break;
        }

        return StarString;
    }
}