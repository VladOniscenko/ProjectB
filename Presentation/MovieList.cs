using ProjectB.Database;
using ProjectB.Models.Movies;
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

    /// <summary>
    /// Opens a menu for the user to select what they want to do. 
    /// </summary>
    public void OpenUserMenu()
    {
        while (true)
        {
            // Console.Clear();
            // Console.WriteLine("Movie List");
            // Console.WriteLine("1. View Movies");
            // Console.WriteLine("2. Exit");
            // Console.Write("Select an option: ");

            // string choice = Console.ReadLine();
            // ask userr to select an option from the menu
            int choice = ShowMenu("Movie List", new List<string> { "View Movies", "Exit" });
            switch (choice)
            {
                case 0:
                    ViewMoviesInRepo(movieRepo);
                    break;
                case 1:
                    return;                
            }
        }
    }

    private void ViewMoviesInRepo(MovieRepository movieRepo)
    {
        // Console.Clear();
        // Console.WriteLine("Movie List");
        // Console.WriteLine("===========");

        List<Movie> movies = movieRepo.GetAllMovies()
            .Where(m => m.ReleaseDate >= DateTime.Today.AddDays(-60) && m.ReleaseDate <= DateTime.Today.AddDays(14))
            .ToList();
        // List<Movie> movies = movieRepo.GetAllMovies().ToList();
        // test
        // Console.WriteLine($"DEBUG: {movies.Count} movies loaded from DB.");
        // Console.ReadKey(); // 

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
            Console.Clear();

            Console.WriteLine("Movie List");
            Console.WriteLine("===========");

            // calculate total pages based on total movies and max movies to show
            var TotalPages = (int)Math.Ceiling((double)movies.Count / maxMoviesToShow);
            // Console.WriteLine($"Showing movies {page * maxMoviesToShow + 1} to {(page + 1) * maxMoviesToShow} of {movies.Count}");
            
            // get movies and show them in the console
            var moviesToShow = movies.Skip(page * maxMoviesToShow).Take(maxMoviesToShow).ToList();
            
            // convert movies to string options to choose from in the menu
            List<string> movieOptions = moviesToShow.Select(m => $"{m.Title} ({m.Runtime} min)").ToList();
            
            // for (int i = 0; i < moviesToShow.Count; i++)
            // {
            //     Console.WriteLine($"{i + 1}.{moviesToShow[i].Title} ({moviesToShow[i].Runtime} min)");
        
            // }

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

            // Console.WriteLine("\nOptions:");
            // Console.WriteLine("N. Next Page");
            // Console.WriteLine("P. Previous Page");
            // Console.WriteLine("Enter bumner to Select Movie");
            // Console.WriteLine("B. Back to Main Menu");

            // Console.Write("Please select an option: ");
            // string choice = Console.ReadLine();

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
                else if (hasNext && selectedMovieIndex == offset + 1)
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

    // static void ShowMovieDetails(Movie movie)
    // {
    //     // test
    //     Console.WriteLine($"DEBUG: Showing details for '{movie.Title}'");
    //     Console.ReadKey(); 
        
    //     while (true)
    //     {
    //         Console.Clear();
    //         Console.WriteLine("Movie Details");
    //         Console.WriteLine("=============");
    //         Console.WriteLine($"Title: {movie.Title}");
    //         Console.WriteLine($"Description: {movie.Description}");
    //         Console.WriteLine($"Runtime: {movie.Runtime} minutes");
    //         Console.WriteLine($"Actors: {movie.Actors}");
    //         Console.WriteLine($"Rating: {movie.Rating}");
    //         Console.WriteLine($"Genre: {movie.Genre}");
    //         Console.WriteLine($"Age Restriction: {movie.AgeRestriction}");
    //         Console.WriteLine($"Release Date: {movie.ReleaseDate.ToShortDateString()}");
    //         Console.WriteLine($"Country: {movie.Country}");

    //         // Console.WriteLine();

    //         // give user the option to buy ticket options
    //         List<string> options = new() { "Buy Ticket", "Back to Movie List" };
    //         int selected = ShowMenu("Choose an option:", options);

    //         if (selected == 0)
    //         {
    //             // ticket bought
    //             Console.Clear();
    //             Console.WriteLine("Ticket purchased successfully!");
    //             Console.WriteLine("\nPress any key to return to the movie list...");
    //             Console.ReadKey();
    //             break;
    //         }
    //         else if (selected == 1)
    //         {
    //             // Back to movie list
    //             break;
    //         }
    //     }
    // }
    static void ShowMovieDetails(Movie movie)
    {
        // Console.WriteLine("DEBUG - Movie Raw Dump:");
        // Console.WriteLine($"Title: {movie.Title}");
        // Console.WriteLine($"Runtime: {movie.Runtime}");
        // Console.WriteLine($"ReleaseDate: {movie.ReleaseDate}");
        // Console.WriteLine("Press any key to continue...");
        // Console.ReadKey();

        // Console.Clear(); // now clear AFTER showing debug info
        Console.Clear();
        Console.WriteLine("Movie Details");
        Console.WriteLine("=============");
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Description: {movie.Description}");
        Console.WriteLine($"Runtime: {movie.Runtime} minutes");
        Console.WriteLine($"Actors: {movie.Actors}");
        Console.WriteLine($"Rating: {movie.Rating}");
        Console.WriteLine($"Genre: {movie.Genre}");
        Console.WriteLine($"Age Restriction: {movie.AgeRestriction}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.ToShortDateString()}");
        Console.WriteLine($"Country: {movie.Country}");
    }

    /// <summary>
    /// Shows a menu with options to purchase the selected movie.
    /// </summary>
    private void ShowPurchaseMenu()
    {
        int startingRow = Console.CursorTop;

        List<string> options = new() { "Buy Ticket", "Back to Movie List" };

        int selected = AddMenuFromStartRow("Choose an option:", options, startingRow);

        if (selected == 0)
        {
            Console.Clear();
            Console.WriteLine("Ticket purchased successfully!");
            Console.WriteLine("\nPress any key to return to the movie list...");
            Console.ReadKey();
            return;
        }
        else if (selected == 1)
        {                        
            // Back to movie list
            return;
        }
    }

    // create method to use keyboard arrows instead of console input 
    private int ShowMenu(string title, List<string> options, bool clearScreen = true)
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();

        do
        {
            if (clearScreen)
            {
                Console.Clear();
            }
            // Console.Clear();
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
}
