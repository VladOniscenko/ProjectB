using Microsoft.Extensions.DependencyInjection;
using ProjectB;
using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;

public class CreateShowtime
{
    const int page = 0;
    const int TotalPages = 1;

    private readonly IServiceProvider _services;
    private readonly ShowtimeLogic _showtimeLogic;
    private readonly AuditoriumLogic _auditoriumLogic;
    private readonly MovieLogic _movieLogic;

    public CreateShowtime()
    {
        _services = Program.Services;
        _showtimeLogic = _services.GetRequiredService<ShowtimeLogic>();
        _auditoriumLogic = _services.GetRequiredService<AuditoriumLogic>();
        _movieLogic = _services.GetRequiredService<MovieLogic>();
    }

    public void Run()
    {
        Console.CursorVisible = false;

        Showtime newShowtime = new Showtime();
        // Object of the selected movie
        Movie selectedMovie;
        while (true)
        {
            Console.Clear();

            Console.WriteLine("╔═════════════════════════════╗");
            Console.WriteLine("║       Create  Showtime      ║");
            Console.WriteLine("╚═════════════════════════════╝");

            Console.SetCursorPosition(0, 24);
            Console.Write("                                                                                     \n");

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║     Press ESC to return back to the menu     ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.SetCursorPosition(0, 4);
            
            Console.WriteLine("\nEnter the name/keyword of the movie you want to create a showtime for.\n");
            string movieName = BaseUI.DrawInputBox("name/keyword", 15, 30, -1, -1);
            if (movieName == null)
            {
                BaseUI.ResetColor();
                return;
            }

            IEnumerable<Movie> movies = _movieLogic.GetMoviesByTitle(movieName);

            Console.Clear();
            
            string selectedOption = SearchMovie("Found the following movies:", movies);
    
            if (selectedOption == "EX") return;
            if (selectedOption == "SA") continue;

            int parsedIndex;
            bool success = int.TryParse(selectedOption, out parsedIndex);
            if (success)
            {
                selectedMovie = movies.FirstOrDefault(x => x.Id == parsedIndex);
                break;
            };
    

            Console.WriteLine("Film not found.");
        }
        
        newShowtime.MovieId = selectedMovie.Id;

        Console.Clear();
        IEnumerable<Auditorium> auditoriums = _auditoriumLogic.GetAllAuditoriums();

        // Returns index of selected option
        // Stores selected option based on selected index into Auditorium object
        // Stores specific property from object (id) into variable
        
        string selectedAuditorium = ShowMenuAuditoriums(
            "Select an auditorium for the previously selected movie.\n\n   Auditorium   | Seats ",
            auditoriums
        );
        
        if (selectedAuditorium == "EX") return;
        
        int auditoriumId;
        bool successA = int.TryParse(selectedAuditorium, out auditoriumId);
        newShowtime.AuditoriumId = auditoriumId;
        
        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\nEnter the date and time for the movie you want to create a showtime for.");
            BaseUI.ColoredText("Datetime format: yyyy-MM-dd HH:mm", ConsoleColor.DarkGray);
            BaseUI.ColoredText("Date: year-month-day (ex: 2027-08-21)", ConsoleColor.DarkGray);
            BaseUI.ColoredText("Time: 24-hour (ex: 23:00)", ConsoleColor.DarkGray);
            Console.SetCursorPosition(0, 6);
            string movieStartTime = Console.ReadLine()?.Trim() ?? "";

            if (!_showtimeLogic.IsMovieStartTimeValid(movieStartTime))
            {
                ConsoleMethods.Error("Something went wrong! Check if the date is correct format (yyyy-MM-dd HH:mm) and is not in the past.");
                continue;
            }

            if (_auditoriumLogic.IsAuditoriumTakenAt(auditoriumId, _showtimeLogic.parsedStartTime, _showtimeLogic.parsedStartTime.AddMinutes(selectedMovie.Runtime)))
            {
                ConsoleMethods.Error("This time slot is already taken. Please choose a different time.");
                continue;
            }

            break;
        }

        // Assign Start Time value & create end time using duration of selected movie and adding it to start time above
        newShowtime.StartTime = _showtimeLogic.parsedStartTime;
        newShowtime.EndTime = newShowtime.StartTime.AddMinutes(selectedMovie.Runtime);
        
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

    public string SearchMovie(string title, IEnumerable<Movie> options )
    {
        
        Dictionary<string, string> movieOptions = new();
        foreach (Movie movie in options) 
        {
            movieOptions.Add(movie.Id.ToString(), movie.Title);
        }
        
        movieOptions.Add("EX", "Exit");
        movieOptions.Add("SA", "Search Again");
        
        return Menu.SelectMenu(title, movieOptions);
    }

    public string ShowMenuAuditoriums(string title, IEnumerable<Auditorium> options )
    {
        Dictionary<string, string> AuditoriumOptions = new();
        foreach (Auditorium auditorium in options) 
        {
            AuditoriumOptions.Add(auditorium.Id.ToString(), auditorium.Name);
        }
        
        AuditoriumOptions.Add("EX", "Exit");
        return Menu.SelectMenu(title, AuditoriumOptions);
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