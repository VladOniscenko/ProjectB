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
        
        // Still need to make it so you got multiple pages
        while (!(ShowtimeLogic.IsMovieIDValid(movieName)))
        {
            Console.WriteLine("\nEnter the name/keyword of the movie you want to create a showtime for.");
            movieName = Console.ReadLine().Trim();
        }

        int selectedMovieIndex = ShowtimeLogic.ShowMenuMovies($"(Page {page + 1}/{TotalPages})\n\nFound the following movie(s):\n\nID ║ Title ║ Year ║ Genres ║ Rating", movies);
        Movie selectedMovie = movies[selectedMovieIndex];
        Console.Clear();

        AuditoriumRepository auditoriumRepository = new AuditoriumRepository();
        List<Auditorium> auditoriums = auditoriumRepository.GetAllAuditoriums().ToList();

        int selectedIndex = ShowtimeLogic.ShowMenuAuditoriums("Select an auditorium for the previously selected movie.", auditoriums);
        Auditorium selectedAuditorium = auditoriums[selectedIndex];
        Console.Clear();

        
    }
}    