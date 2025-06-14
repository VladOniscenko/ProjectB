namespace ProjectB.Models;

public class SeatReservation : BaseModel
{
    public int SeatId { get; set; }
    public int ReservationId { get; set; }
    public int ShowtimeId { get; set; }
    public string TicketType { get; set; }
    
    public override string ToString()
    {
        return $"Seat {SeatId} in Reservation {ReservationId}";
    }
}