using ProjectB.Database;
using ProjectB.Models;
using ProjectB.DataAccess;
using System.Runtime.InteropServices;
using System.Collections;
namespace ProjectB.Presentation;

public class MovieList
{
    private MovieRepository movieRepo;
    public MovieList(MovieRepository movieRepo)
    {
        this.movieRepo = movieRepo;
    }

    public void OpenUserMenu()
    {
        ViewMoviesInRepo(movieRepo);
    }

    // Takes from the Repo and makes a list of movies
    private void ViewMoviesInRepo(MovieRepository movieRepo)
    {
        List<Movie> movies = movieRepo.GetAllMovies()
            .Where(m => m.ReleaseDate >= DateTime.Today.AddDays(-60) && m.ReleaseDate <= DateTime.Today.AddDays(14))
            .ToList();

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies available this week.");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey(); 
            return;
        }
        
        const int maxMoviesToShow = 5;
        int page = 0;

        while (true)
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine("Movie List");
            Console.WriteLine("===========");

            // calculate total pages based on total movies and max movies to show
            var TotalPages = (int)Math.Ceiling((double)movies.Count / maxMoviesToShow);
            // Console.WriteLine($"Showing movies {page * maxMoviesToShow + 1} to {(page + 1) * maxMoviesToShow} of {movies.Count}");
            
            // get movies and show them in the console
            var moviesToShow = movies.Skip(page * maxMoviesToShow).Take(maxMoviesToShow).ToList();
            
            // convert movies to string options to choose from in the menu
            List<string> movieOptions = moviesToShow.Select(m => $"{m.Title} | ({m.Runtime} min) | Genres: {m.Genre} | {CalcStars(m.Rating)}").ToList();

            // make optiions for the previous and next pagge
            if (page > 0){
                Console.WriteLine("===========");
                movieOptions.Add("Previous Page");
            }
            if ((page + 1) * maxMoviesToShow < movies.Count){
                Console.WriteLine("===========");
                movieOptions.Add("Next Page");
            }
            movieOptions.Add("Back to Main Menu");
            // show the movies in the menu
            int selectedMovieIndex = ShowMenu($"Showing movies (Page {page + 1}/{TotalPages})", movieOptions);

            if (selectedMovieIndex <moviesToShow.Count)
            {
                // show the movie details
                ShowMovieDetails(moviesToShow[selectedMovieIndex]);
                ShowPurchaseMenu();
            }
            else
            {
                int offset = moviesToShow.Count;
                bool hasPrev = page > 0;
                bool hasNext = (page + 1) * maxMoviesToShow < movies.Count;
                if (hasPrev && selectedMovieIndex == offset)
                {
                    page--;
                }
                else if (hasNext && selectedMovieIndex == offset)
                {
                    page++;
                }
                else if (
                    (!hasNext && selectedMovieIndex == offset) ||
                    (hasPrev && hasNext && selectedMovieIndex == offset + 2) ||
                    (!hasPrev && hasNext && selectedMovieIndex == offset +1)
                )
                {
                    break;
                }
            }


        }
    }

    // Shows the movie in a nice format
    static void ShowMovieDetails(Movie movie)
    {
        Console.Clear();
        Console.WriteLine("=== MOVIE DETAILS ===");
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Title           : {movie.Title}");
        Console.WriteLine($"Description     : {movie.Description}");
        Console.WriteLine($"Runtime         : {movie.Runtime} minutes");
        Console.WriteLine($"Actors          : {movie.Actors}");
        Console.WriteLine($"Rating          : {CalcStars(movie.Rating)} ({movie.Rating}/5)");
        Console.WriteLine($"Genre           : {movie.Genre}");
        Console.WriteLine($"Age Restriction : {movie.AgeRestriction}");
        Console.WriteLine($"Release Date    : {movie.ReleaseDate.ToShortDateString()}");
        Console.WriteLine($"Country         : {movie.Country}");
        Console.WriteLine("------------------------------");
    }

    /// Shows a menu with options to purchase the selected movie.
    private void ShowPurchaseMenu()
    {
        int startingRow = Console.CursorTop;

        List<string> options = new() { "Buy Ticket", "Back to Movie List" };

        int selected = AddMenuFromStartRow("Choose an option:", options, startingRow);

        if (selected == 0)
        {
            Console.WriteLine("Ticket purchased successfully!");
            Console.WriteLine("\nPress any key to return to the movie list...");
            Console.ReadKey();
            // Start ticket buying function here
            Menu.RunMenu();
            return;
        }
        else if (selected == 1)
        {                        
            Console.Clear();
            return;
        }
    }

    // create method to use keyboard arrows instead of console input 
    private int ShowMenu(string title, List<string> options )
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();
 
        do
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine(title);
            Console.WriteLine(new string('=', title.Length));

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
                Console.WriteLine(new string('-', options[i].Length));
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

        } while (key != ConsoleKey.Enter);

        return selected;
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
            ClearAllLinesDownFrom(startRow - (rowBuffer +1));

            // Set the cursor in the console to our starting row.
            Console.SetCursorPosition(0, startRow - (rowBuffer +1));

            // Empty line for spacing.
            Console.WriteLine();

            // Write the menu itself
            Console.WriteLine(title);
            Console.WriteLine(new string('=', title.Length));

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
