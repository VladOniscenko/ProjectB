using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ReservationFlow
{
    private ReservationLogic Reservation;
    private Showtime SelectedShowtime;
    private Movie SelectedMovie;
    
    
    public ReservationFlow(int movieId)
    {

        SelectedMovie = MovieLogic.Find(movieId);
        if (SelectedMovie == null)
        {
            ConsoleMethods.Error("Movie not found");
            return;
        }
        
        Reservation = new ReservationLogic(SelectedMovie);
    }

    public void Run()
    {
        ConsoleMethods.Error("Not implemented yet!");
        // 1. select showtime
    
        // 2. select seats
    
        // 3. select tickets type
        
        // 4. logged in? go to step 9
        
        // 5. Ask to login or to register
        
        // 6. confirmation (total price seats etc.)
        
        // 7. pay
        
    }

    
}