using ProjectB.Models;

namespace ProjectB.Logic;

public class ReservationLogic
{
    private static List<Seat> SelectedSeats { get; set; } = new();

    public static void AddSeat(Seat seat) => SelectedSeats.Add(seat);
    public static void RemoveSeat(Seat seat) => SelectedSeats.Remove(seat);
}