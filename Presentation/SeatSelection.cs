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
        int? currentRow = null;

        foreach (var seat in seats)
        {
            
            if (currentRow != seat.Row)
            {
                if (currentRow != null)
                    Console.WriteLine();

                Console.Write($"Row {seat.Row:D2} | ");
                currentRow = seat.Row;
            }
            
            string type = "--";
            if (seat.Type == "love_seat")
            {
                type = "LV";
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (seat.Type == "vip")
            {
                type = "VP";
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (seat.Type == "normal")
            {
                type = "NO";
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            Console.Write(seat.Active == 1 ? $"{seat.Number:D2} " : "-- ");
            Console.ResetColor();
        }
        
        Console.ReadLine();
    }
}