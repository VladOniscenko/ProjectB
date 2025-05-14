namespace ProjectB.Models;

public class Auditorium
{
    public int Id { get; set; }
    public int TotalSeats { get; set; }
    public string Name { get; set; }
    public int CinemaId { get; set; }

    public Auditorium(){ }
    public Auditorium(string name, int totalSeats, int cinemaId)
    {
        Name = name;
        TotalSeats = totalSeats;
        CinemaId = cinemaId;
    }

    public override string ToString()
    {       
        return $"{Name} | {TotalSeats}";
    }
}