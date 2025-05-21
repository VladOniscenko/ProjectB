using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SelectShowtime
{
    private Movie _movie;
    private IEnumerable<Showtime> _availableShowtimes;

    private readonly IServiceProvider _services;
    private readonly ShowtimeLogic _showtimeService;
    
    public SelectShowtime(Movie? movie)
    {
        _services = Program.Services;
        _movie = movie;
        
        // get show times of the movie
        _showtimeService = _services.GetRequiredService<ShowtimeLogic>();
        _availableShowtimes = _showtimeService.GetShowtimesByMovieId(_movie.Id);
    }

    public Showtime? Run()
    {
        Console.Clear();

        if (_availableShowtimes == null || _availableShowtimes.Count() == 0)
        {
            ConsoleMethods.Error("No availability found");
            return null;
        }
        
        var showtimeOptions = _availableShowtimes.ToDictionary(
            s => s.Id.ToString(),
            s => $"{s.StartTime} | {s.Auditorium.Name}"
        );

        showtimeOptions.Add("S", "Return to movie selection");
        
        // show the movies in the menu
        var selectedOption = Menu.SelectMenu($"Select a movie show time", showtimeOptions);
        if (selectedOption == "S")
        {
            return null;
        }
        
        return _availableShowtimes.FirstOrDefault(m => m.Id == int.Parse(selectedOption));
    }
}