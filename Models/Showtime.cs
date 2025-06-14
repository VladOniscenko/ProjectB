namespace ProjectB.Models;

public class Showtime : BaseModel
{
    public int MovieId { get; set; }
    public int AuditoriumId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Auditorium? Auditorium { get; set; }

    public override string ToString()
    {
        return $"Showtime for MovieId {MovieId} in Auditorium {AuditoriumId} at {StartTime}";
    }
}