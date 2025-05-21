namespace ProjectB.Models;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ShowtimeId { get; set; }
    public DateTime CreationDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string? PaymentMethod { get; set; }
}





