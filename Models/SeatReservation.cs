namespace ProjectB.Models;

public class SeatReservation
{
    public int Id { get; set; }
    public int SeatId { get; set; }
    public int ReservationId { get; set; }
    public int ShowtimeId { get; set; }
}