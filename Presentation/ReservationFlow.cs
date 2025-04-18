using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ReservationFlow
{
    private ReservationLogic Reservation;
    private Showtime SelectedShowtime;
    private Movie SelectedMovie;

    public ReservationFlow(Movie movie)
    {
        
        if (movie == null)
        {
            ConsoleMethods.Error("Movie not found");
            return;
        }
        
        SelectedMovie = movie;
        Reservation = new ReservationLogic(movie);
    }
    
    public ReservationFlow(int movieId) : this(MovieLogic.Find(movieId)) { }

    public void Run()
    {
        // 1. select showtime
        SelectShowtime selectShowtime = new SelectShowtime(SelectedMovie);
        int? showtimeId = selectShowtime.Run();

        if (showtimeId < 0)
        {
            return;
        }

        // Reservation.SelectShowtime(showtimeId);
        
        Console.WriteLine(showtimeId);

        // 2. select seats
        // SeatSelection seatSelection = new SeatSelection();
        
        // 3. select tickets type

        // 4. logged in? go to step 9

        // 5. Ask to login or to register

        // 6. confirmation (total price seats etc.)

        // 7. pay

    }

    
}