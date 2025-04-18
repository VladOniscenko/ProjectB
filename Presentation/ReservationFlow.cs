using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ReservationFlow
{
    private ReservationLogic Reservation;
    private Showtime SelectedShowtime;
    private Movie SelectedMovie;
    
    public ReservationFlow(int movieId) : this(MovieLogic.Find(movieId)) { }
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
    

    public void Run()
    {
        // 1. select showtime
        SelectShowtime selectShowtime = new SelectShowtime(SelectedMovie);
        Showtime? showtime = selectShowtime.Run();
        if (showtime == null)
        {
            return;
        }
        
        if (!Reservation.SelectShowtime(showtime))
        {
            ConsoleMethods.Error("Show time is not available");
            return;
        }

        // 2. select seats
        SeatSelection seatSelection = new SeatSelection(showtime);
        IEnumerable<Seat>? seats = seatSelection.Run();
        
        // 3. select tickets type

        // 4. logged in? go to step 9

        // 5. Ask to login or to register

        // 6. confirmation (total price seats etc.)

        // 7. pay

    }

    
}