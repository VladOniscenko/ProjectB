using Microsoft.Extensions.DependencyInjection;
using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

public class CreateShowtime
{
    const int page = 0;
    const int TotalPages = 1;

    private readonly IServiceProvider _services;
    private readonly IShowtimeService _showtimeLogic;

    public CreateShowtime(IServiceProvider services)
    {
        _services = services;
        _showtimeLogic = _services.GetRequiredService<IShowtimeService>();
    }

    public void Run()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();

        Console.WriteLine("╔═════════════════════════════╗");
        Console.WriteLine("║       Create  Showtime      ║");
        Console.WriteLine("╚═════════════════════════════╝");

        Showtime newShowtime = new Showtime();

        Console.WriteLine("\nEnter the name/keyword of the movie you want to create a showtime for.");
        string movieName = Console.ReadLine().Trim();

        Console.Clear();

        MovieRepository movieRepository = new MovieRepository();
        List<Movie> movies = movieRepository.GetMoviesByTitle(movieName);

        // Dunno how to make it so it has multiple pages.
        while (!_showtimeLogic.IsMovieIDValid(movieName))
        {
            Console.Clear();
            BaseUI.ShowErrorMessage("No input given. Please try again.\n", 0);
            Console.WriteLine("Enter the name/keyword of the movie you want to create a showtime for.");
            movieName = Console.ReadLine().Trim();
        }

        // If not this, after incorrect input, menu won't search movies based on user input.
        movies = movieRepository.GetMoviesByTitle(movieName);

        Console.Clear();

        int selectedMovieIndex = ShowMenuMovies("Found the following movies:", movies);

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
            ShowMenuAuditoriums(
            "Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats | ID",
            auditoriums)
        ].Id;

        Console.Clear();

        Console.WriteLine("\nEnter the date and time (yyyy-MM-dd HH:mm:ss) for the movie you want to create a showtime for.");
        string movieStartTime = Console.ReadLine().Trim();

        while (!_showtimeLogic.IsMovieStartTimeValid(movieStartTime))
        {
            Console.Clear();
            BaseUI.ShowErrorMessage("Incorrect date, given. Please try again.\n", 0);
            Console.WriteLine("\nEnter the date and time (yyyy-MM-dd HH:mm:ss) for the movie you want to create a showtime for.");
            movieStartTime = Console.ReadLine().Trim();
        }

        // Assign Start Time value & create end time using duration of selected movie and adding it to start time above
        newShowtime.StartTime = _showtimeLogic.parsedStartTime;
        int movieDuration = selectedMovie.Runtime;
        newShowtime.EndTime = newShowtime.StartTime.AddMinutes(movieDuration);
        
        Console.Clear();

        // If Yes
        if (CheckIfDataCorrect(selectedMovie.Title, newShowtime.AuditoriumId, newShowtime.StartTime, newShowtime.EndTime))
        {
            _showtimeLogic.CreateShowtime(newShowtime);
            Console.Clear();
            Console.WriteLine("New showtime has been created!");
            Thread.Sleep(1000);
            Console.WriteLine("Would you like to add another showtime?");

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

// =============================================================================================================================================

    public int ShowMenuMovies(string title, List<Movie> options )
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();
 
        do
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine(title);
            Console.WriteLine(new string('═', title.Length));

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($">> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {options[i]}");
                }
                // Spaces between titles looks ugly tbh.
                // Console.WriteLine(new string('-', options[i].Length));
            }

            if (options.Count == 0)
            {   Console.WriteLine();
                BaseUI.ShowErrorMessage("\nNo movies found with this title/keyword.", 6);
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

    public int ShowMenuAuditoriums(string title, List<Auditorium> options )
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();
 
        do
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine(title);
            Console.WriteLine(new string('═', title.Length));

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($">> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {options[i]}");
                }
                // Spaces between titles looks ugly tbh.
                // Console.WriteLine(new string('-', options[i].Length));
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

    public bool CheckIfDataCorrect(string movie, int auditorium, DateTime movieStartTime, DateTime movieEndTime)
    {
        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine(
            $"    Movie:                {movie}\n    Auditorium:           {auditorium} \n    Start time:           {movieStartTime}\n    End time:             {movieEndTime}");
        Console.WriteLine("\n   Is this correct?");
        Console.Write("    ");
        return BaseUI.BasicYesOrNo();
    }
}