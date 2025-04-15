namespace ProjectB.Models;

public class Seat
{
    public int Id { get; set; }
    public int AuditoriumId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public int Active { get; set; }
    public string Type { get; set; }
}