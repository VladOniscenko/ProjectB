namespace ProjectB.Models;

public class Reservation : BaseModel
{
    public int UserId { get; set; }
    public int ShowtimeId { get; set; }
    public DateTime CreationDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string? PaymentMethod { get; set; }

    public override string ToString()
    {
        return $"UserId: {UserId}, ReservationId: {Id}";
    }
}





