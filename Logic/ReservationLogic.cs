using ProjectB.Models;

namespace ProjectB.Logic;

public class ReservationLogic
{
    private Showtime SelectedShowtime;
    private Movie SelectedMovie;
    private List<Seat> SelectedSeats { get; set; } = new();

    public ReservationLogic(Movie movie)
    {
        SelectedMovie = movie;
    }
    
    public bool SelectShowtime(Showtime? showtime)
    {
        if (showtime == null || showtime.StartTime < DateTime.Now)
        {
            return false;
        }

        SelectedShowtime = showtime;
        return true;
    }
    
    // private void AddSeat(Seat seat)
    // {
    //     // todo check if seat is not taken
    //     if (seat.Taken == 0)
    //     {
    //         seat.Selected = true;
    //         SelectedSeats.Add(seat);
    //     }
    // }
    //
    // private void RemoveSeat(Seat seat)
    // {
    //     if (SelectedSeats.Contains(seat))
    //     {
    //         seat.Selected = false;
    //         SelectedSeats.Remove(seat);   
    //     }
    // }
    //
    // public void AddOrRemoveSeat(Seat seat)
    // {
    //     if (seat.Selected)
    //     {
    //         RemoveSeat(seat);
    //         return;
    //     }
    //     AddSeat(seat);
    // }

    
    
    
    
    
    
    
}