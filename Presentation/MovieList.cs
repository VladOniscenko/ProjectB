using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
using System.Runtime.InteropServices;
using System.Collections;
namespace ProjectB;

public class MovieList
{
    public static void ViewMovies()
    {
        // initialize the database
        DbFactory.InitializeDatabase();
        // create a new instance of the movie repository
        var movieRepo = new MovieRepository();

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
                    ViewMovies(movieRepo);
                    break;
                case 1:
                    return;                

            }
        }
    }

    static void ViewMovies(MovieRepository movieRepo)
    {
        // Console.Clear();
        // Console.WriteLine("Movie List");
        // Console.WriteLine("===========");

        // List<Movie> movies = movieRepo.GetAllMovies()
        //     .Where(m => m.ReleaseDate >= DateTime.Today.AddDays(-7) && m.ReleaseDate <= DateTime.Today.AddDays(14))
        //     .ToList();
        List<Movie> movies = movieRepo.GetAllMovies().ToList();

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
            var TptalPages = (int)Math.Ceiling((double)movies.Count / maxMoviesToShow);
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
            if (page > 0)
                Console.WriteLine("===========");
                movieOptions.Add("Previous Page");
            if ((page + 1) * maxMoviesToShow < movies.Count)
                Console.WriteLine("===========");
                movieOptions.Add("Next Page");
            movieOptions.Add("Back to Main Menu");
            // show the movies in the menu
            int selectedMovieIndex = ShowMenu($"Showing movies (Page {page + 1}/{TptalPages})", movieOptions);

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
            }
            else{
                int offsett = moviesToShow.Count;
                bool hasPrev = page > 0;
                bool hasNext = (page + 1) * maxMoviesToShow < movies.Count;
                if (hasPrev && selectedMovieIndex == offsett)
                {
                    page--;
                }
                else if (hasNext && selectedMovieIndex == offsett + 1)
                {
                    page++;
                }
                else if (
                    (!hasNext && selectedMovieIndex == offsett) ||
                    (hasPrev && hasNext && selectedMovieIndex == offsett + 2) ||
                    (!hasPrev && hasNext && selectedMovieIndex == offsett +1)
                )
                {
                    break;
                }
            }


        }
    }

    static void ShowMovieDetails(Movie movie)
    {
        while (true)
        {
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

            Console.WriteLine();

            // give user the option to buy ticket options
            List<string> options = new() { "Buy Ticket", "Back to Movie List" };
            int selected = ShowMenu("Choose an option:", options);

            if (selected == 0)
            {
                // ticket bought
                Console.Clear();
                Console.WriteLine("Ticket purchased successfully!");
                Console.WriteLine("\nPress any key to return to the movie list...");
                Console.ReadKey();
                break;
            }
            else if (selected == 1)
            {
                // Back to movie list
                break;
            }
        }
    }

    // create method to use keyboard arrows instead of console input 
    static int ShowMenu(string title, List<string> options)
    {
        int selected = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
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

}