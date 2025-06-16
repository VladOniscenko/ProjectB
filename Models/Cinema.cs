namespace ProjectB.Models;

public class Cinema : BaseModel
{
    public const string MainCinemaName = "Our Cinema";
    public string CinemaName { get; set; }
    // public List<Auditorium> Auditoriums { get; set; }
    // public const double BasicPrice = 14.0;

    public Cinema() {}
    public Cinema(string name)
    {
        CinemaName = name;
    }
    
    public override string ToString()
    {
        return CinemaName;
    }
}