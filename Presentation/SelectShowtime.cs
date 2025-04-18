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

    public int Run()
    {
        Console.Clear();
        
        var showtimeOptions = AvailableShowtimes.ToDictionary(
            s => s.Id.ToString(),
            s => $"{s.StartTime}"
        );

        showtimeOptions.Add("S", "Stop");
        
        // show the movies in the menu
        var selectedOption = Menu.SelectMenu($"Select a movie show time", showtimeOptions);
        switch (selectedOption)
        {
            case "S":
                return -1;
            default:
                Showtime? showtime = AvailableShowtimes.FirstOrDefault(m => m.Id == int.Parse(selectedOption));

                return (showtime == null) ? -1 : showtime.Id;
        }
    }
}