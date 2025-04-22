using Microsoft.Extensions.DependencyInjection;
using ProjectB.DataAccess;
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
            Console.WriteLine("Enter the name/keyword of the movie you want to create a showtime for.");
            movieName = Console.ReadLine().Trim();
        }

        int selectedMovieIndex = _showtimeLogic.ShowMenuMovies("Found the following movies:", movies);

        // Object of the selected movie
        Movie selectedMovie = movies[selectedMovieIndex];
        newShowtime.MovieId = selectedMovie.Id;

        Console.Clear();

        AuditoriumRepository auditoriumRepository = new AuditoriumRepository();
        var auditoriums = auditoriumRepository.GetAllAuditoriums().ToList();

        // Returns index of selected option
        // Stores selected option based on selected index into Auditorium object
        // Stores specific property from object (id) into variable

        newShowtime.AuditoriumId = auditoriums[
            _showtimeLogic.ShowMenuAuditoriums(
                "Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats | ID",
                auditoriums)
        ].Id;

        // Create startTime + endTime here?

        Console.Clear();

        // If Yes
        if (_showtimeLogic.CheckIfDataCorrect(selectedMovie.Title, newShowtime.AuditoriumId))
        {
            _showtimeLogic.CreateShowtime(newShowtime);
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
/// 1) Make page function, well, functional. I've got an idea as to how but, I'm not sure. Didn't Angel already do this?
/// 2) Fix second BasicYesOrNo menu 'bug'. Has to do with the placement of the boxes when you use arrowkeys, but it's set in stone.
/// </summary>