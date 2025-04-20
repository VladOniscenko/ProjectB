using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Models;

public static class CreateMovieShowtime
{
    const int page = 0;
    const int TotalPages = 1;

    public static void Run()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();

        Console.WriteLine("╔═════════════════════════════╗");
        Console.WriteLine("║    Create Movie Showtime    ║");
        Console.WriteLine("╚═════════════════════════════╝");

        Showtime newShowtime = new Showtime();

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
        
        newShowtime.MovieId = movies[
        ShowtimeLogic.ShowMenuMovies("Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats  | ID", movies)
        ].Id;

        int selectedMovieIndex = ShowtimeLogic.ShowMenuMovies(
        "Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats  | ID", 
        movies
        );

        // Object of the selected movie
        Movie selectedMovie = movies[selectedMovieIndex];
        newShowtime.MovieId = selectedMovie.Id;

        Console.Clear();

        AuditoriumRepository auditoriumRepository = new AuditoriumRepository();
        List<Auditorium> auditoriums = auditoriumRepository.GetAllAuditoriums().ToList();

        // Returns index of selected option
        // Stores selected option based on selected index into Auditorium object
        // Stores specific property from object (id) into variable

        newShowtime.AuditoriumId = auditoriums[
        ShowtimeLogic.ShowMenuAuditoriums("Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats  | ID", auditoriums)
        ].Id;

        // Create startTime + endTime here?

        Console.Clear();

        // If Yes
        if (ShowtimeLogic.CheckIfDataCorrect(selectedMovie.Title, newShowtime.AuditoriumId))
        {
            ShowtimeLogic.CreateShowtime(newShowtime);
            Console.Clear();
            Console.WriteLine("New showtime has been created!");
            Thread.Sleep(1000);
            Console.WriteLine("Would you like to add another showtime?");

            // Issue: BasicYesOrNo menu is glitched when used a second time for some reason.
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

    /// <summary>
    /// Things to still do:
    /// 1) Make page functionality, well, functional.
    /// 2) Fix second BasicYesOrNo menu 'bug'
    /// </summary>