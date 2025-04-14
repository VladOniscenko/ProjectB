using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public static class SeatSelection
{
    public static void SelectSeats(int showtime_id)
    {
        Showtime? showtime = ShowtimeLogic.GetById(showtime_id);
        Movie? movie = MovieLogic.GetById(showtime.MovieId);
        Auditorium? auditorium = ShowtimeLogic.GetById(showtime.AuditoriumId);

        if (showtime == null)
        {
            ConsoleMethods.Error("Showtime was not found!");
            return;
        }
        
        ConsoleMethods.Success("Showtime was found!" + showtime);
    }
}