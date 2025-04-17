using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Models;

public static class CreateMovieShowtime
{
    const int page = 0;
    const int TotalPages = 3;

    public static void Run()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();

        Console.WriteLine("╔═════════════════════════════╗");
        Console.WriteLine("║    Create Movie Showtime    ║");
        Console.WriteLine("╚═════════════════════════════╝");

        Console.WriteLine("\nEnter the name/keyword of the movie you want to create a showtime for.");
        string movieName = Console.ReadLine().Trim();
        Console.Clear();

        MovieRepository movieRepository = new MovieRepository();
        List<Movie> movies = movieRepository.GetMoviesByTitle(movieName);
        int selectedMovieIndex = ShowtimeLogic.ShowMenu($"(Page {page + 1}/{TotalPages})\n\nFound the following movie(s):\n\nID ║ Title ║ Year ║ Genres ║ Rating", movies);

        // int index = 1;
        // Console.WriteLine();
        // foreach (Movie movie in movies)
        // {
        //     Console.WriteLine($"[{index}] {movie.Title}");
        //     index++;
        // }

        Console.ReadLine().Trim();

        
    }
}    