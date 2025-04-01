namespace ProjectB.Models.Cinemas;

public class Cinema
{
    public const string MainCinemaName = "Our Cinema";
    public int Id { get; set; }
    public string CinemaName { get; set; }
    // public List<Auditorium> Auditoriums { get; set; }
    // public const double BasicPrice = 14.0;

    public Cinema() {}
    public Cinema(string name)
    {
        CinemaName = name;
    }
}