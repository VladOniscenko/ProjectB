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

        MovieRepository movieRepository = new MovieRepository();
        List<Movie> movies = movieRepository.GetMoviesByTitle(movieName);

        int index = 1;
        Console.WriteLine();
        foreach (Movie movie in movies)
        {
            Console.WriteLine($"[{index}] {movie.Title}");
            index++;
        }

        Console.ReadLine().Trim();

        // int selectedMovieIndex = ShowtimeLogic.ShowMenu($"Showing movies (Page {page + 1}/{TotalPages})", movieOptions);
    }
}    