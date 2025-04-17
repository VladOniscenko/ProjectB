using ProjectB.DataAccess;

public static class CreateMovieShowtime
{
    public static void Run()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();

        Console.WriteLine("╔═════════════════════════════╗");
        Console.WriteLine("║    Create Movie Showtime    ║");
        Console.WriteLine("╚═════════════════════════════╝");

        Console.WriteLine("\nEnter the name or a keyword of the movie you want to create a showtime for.");
        string movieName = Console.ReadLine().Trim();

        // Print list of movies with that name/keyword.
        MovieRepository movieRepo = new MovieRepository();
        Console.WriteLine(movieRepo.GetMoviesByTitle(movieName, 6));
        Console.ReadLine();
    }
}    