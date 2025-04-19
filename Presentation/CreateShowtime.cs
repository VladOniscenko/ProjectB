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
        
        // Dunno how to make it so it has multiple pages.
        while (!(ShowtimeLogic.IsMovieIDValid(movieName)))
        {
            Console.WriteLine("Enter the name/keyword of the movie you want to create a showtime for.");
            movieName = Console.ReadLine().Trim();
        }

        int selectedMovieIndex = ShowtimeLogic.ShowMenuMovies($"(Page {page + 1}/{TotalPages})\n\nFound the following movie(s):\n\nID ║ Title ║ Year ║ Genres ║ Rating", movies);
        Movie selectedMovie = movies[selectedMovieIndex];
        Console.Clear();

        AuditoriumRepository auditoriumRepository = new AuditoriumRepository();
        List<Auditorium> auditoriums = auditoriumRepository.GetAllAuditoriums().ToList();

        int selectedAuditoriumIndex = ShowtimeLogic.ShowMenuAuditoriums("Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats  | ID", auditoriums);
        Auditorium selectedAuditorium = auditoriums[selectedAuditoriumIndex];
        Console.Clear();

        // If Yes
        if (ShowtimeLogic.CheckIfDataCorrect(selectedMovie, selectedAuditorium))
        {
            // Query to add showtime to database
            Console.Clear();
            Console.WriteLine("New showtime has been created!");
            Thread.Sleep(1000);
            Console.WriteLine("Would you like to add another showtime?");

            // Issue: BaseicYesOrNo menu is glitched when used twice in a row.
            if (BaseUI.BasicYesOrNo())
            {
                Run();
            }
        }
        // If No
        else
        {
            Run();
        }
        
    }
}    

// Only thing left: How to add begin showtime and end showtime?