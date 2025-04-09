namespace ProjectB.Models;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ShowtimeId { get; set; }
    public DateTime CreationDate { get; set; }
}