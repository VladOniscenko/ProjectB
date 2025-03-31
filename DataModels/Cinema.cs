namespace ProjectB.DataModels;

public class Cinema
{
    public int Id { get; }
    public string CinemaName { get; set; }
    public List<Auditorium> Auditoriums { get; set; }
    public const double BasicPrice = 14.0;

    public Cinema(int id, string name, List<Auditorium> auditoriums)
    {
        Id = id;
        CinemaName = name;
        Auditoriums = auditoriums;
    }

    public static string Table()
    {
        return @"
            CREATE TABLE IF NOT EXISTS cinema (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CinemaName TEXT NOT NULL
            );
        ";
    }
}