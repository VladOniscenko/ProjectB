using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {

        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        // As a customer I want to be able to select a movie, in order to view information or buy tickets for the movie #12
        // Gives customer two options: Buy ticket and View information
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Movie Ticket Booking System!");
            Console.WriteLine("1. View Movies");
            Console.WriteLine("2. Exit");
            Console.Write("Please select an option: ");
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    ViewMovies(movieRepo);
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    static void ViewMovies(MovieRepository movieRepo)
    {
        Console.Clear();
        List<Movie> movies = movieRepo.GetAllMovies().ToList();
        
        if (movies.Count == 0)
        {
            Console.WriteLine("No movies found.");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey(); 
            return;
        }
                
        Console.WriteLine(" Available Movies:");
        for (int i = 0; i < movies.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {movies[i].Title}");
        }

        Console.WriteLine("Select a movie by number to view more information or buy a ticket.");
        if (!int.TryParse(Console.ReadLine(), out int movieIndex) && movieIndex < 0 && movieIndex > movies.Count)
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey();
            return;
        }

        // if (movieIndex == 0) return;
        ShowMovieDetails(movies[movieIndex - 1]);
    }

    static void ShowMovieDetails(Movie movie)
    {
        Console.Clear();
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Genre: {movie.Genre}");
        Console.WriteLine($"Runtime: {movie.Runtime} minutes");
        Console.WriteLine($"Age Restriction: {movie.AgeRestriction}");
        Console.WriteLine($"Release Date: {movie.ReleaseDate.ToShortDateString()}");
        Console.WriteLine($"Actors: {movie.Actors}");
        Console.WriteLine($"Rating: {movie.Rating}");
        Console.WriteLine($"Description: {movie.Description}");


        // Here you can add functionality to buy a ticket
        Console.WriteLine("Would you like to buy a ticket? (y/n)");
        string buyTicketChoice = Console.ReadLine();
        if (buyTicketChoice.ToLower() == "y")
        {
            // Call the method to buy a ticket
            Console.WriteLine("Ticket purchased successfully!");
        }
        else
        {
            Console.WriteLine("Returning to movie list...");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey();
            return;
        }
    }
}
        
        // foreach (Movie movie in movieRepo.GetBestAndNewestMovies())
        // {
        //     Console.WriteLine($"{movie}");
        // }

