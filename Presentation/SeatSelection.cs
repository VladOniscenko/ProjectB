using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public static class SeatSelection
{
    public static void SelectSeats(int showtimeId)
    {
        Showtime? showtime = ShowtimeLogic.Find(showtimeId);
        if (showtime == null)
        {
            ConsoleMethods.Error("Showtime was not found!");
            return;
        }
        
        Movie? movie = MovieLogic.Find(showtime.MovieId);
        Auditorium? auditorium = AuditoriumLogic.Find(showtime.AuditoriumId);
        if (movie == null || auditorium == null)
        {
            ConsoleMethods.Error("Movie or Auditorium was not found!");
            return;
        }

        IEnumerable<Seat> seats = SeatLogic.GetSeatsByAuditorium(auditorium.Id);
        int row = 1;
        foreach (var seat in seats)
        {
            if (row != seat.Row)
            {
                Console.WriteLine("\n");
                row++;
            }
            
            
            Console.Write(seat.Number + " ");
        }
        
        Console.ReadLine();
    }
}