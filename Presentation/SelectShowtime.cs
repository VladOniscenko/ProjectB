using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class SelectShowtime
{
    private Movie SelectedMovie;
    private IEnumerable<Showtime> AvailableShowtimes;
    
    public SelectShowtime(int movieId) : this(MovieLogic.Find(movieId)){ }

    public SelectShowtime(Movie? movie)
    {
        if (movie == null)
        {
            ConsoleMethods.Error("Movie not found");
            return;
        }
        SelectedMovie = movie;
        
        // get show times of the movie
        AvailableShowtimes = ShowtimeLogic.GetShowtimesByMovieId(SelectedMovie.Id);
    }

    public Showtime? Run()
    {
        Console.Clear();

        if (AvailableShowtimes.Count() == 0)
        {
            ConsoleMethods.Error("No availability found");
            return null;
        }
        
        var showtimeOptions = AvailableShowtimes.ToDictionary(
            s => s.Id.ToString(),
            s => $"{s.StartTime}"
        );

        showtimeOptions.Add("S", "Stop");
        
        // show the movies in the menu
        var selectedOption = Menu.SelectMenu($"Select a movie show time", showtimeOptions);
        if (selectedOption == "S")
        {
            return null;
        }
        
        return AvailableShowtimes.FirstOrDefault(m => m.Id == int.Parse(selectedOption));
    }
}