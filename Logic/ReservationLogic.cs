using ProjectB.Models;

namespace ProjectB.Logic;

public class ReservationLogic
{
    private static List<Seat> SelectedSeats { get; set; } = new();

    private static void AddSeat(Seat seat)
    {
        if (seat.Taken == 0)
        {
            seat.Selected = true;
            SelectedSeats.Add(seat);
        }
    }

    private static void RemoveSeat(Seat seat)
    {
        if (SelectedSeats.Contains(seat))
        {
            seat.Selected = false;
            SelectedSeats.Remove(seat);   
        }
    }

    public static void AddOrRemoveSeat(Seat seat)
    {
        if (seat.Selected)
        {
            RemoveSeat(seat);
            return;
        }
        
        AddSeat(seat);
    }
}