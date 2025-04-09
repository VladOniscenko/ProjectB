namespace ProjectB.Models;

public class Showtime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int AuditoriumId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}