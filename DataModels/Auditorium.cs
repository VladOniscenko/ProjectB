namespace ProjectB.DataModels;

public class Auditorium
{
    public int Id { get; set; }
    public int TotalSeats { get; set; }
    public List<Seat> Seats { get; set; }

    // Constructor for Dapper
    public Auditorium() {}

    public Auditorium(int id, int totalSeats, List<Seat> seats)
    {
        Id = id;
        TotalSeats = totalSeats;
        Seats = seats ?? new List<Seat>();
    }

    public static string Table()
    {
        return @"
            CREATE TABLE IF NOT EXISTS auditorium (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TotalSeats INTEGER NOT NULL,
                CinemaId INTEGER NOT NULL,
                FOREIGN KEY (CinemaId) REFERENCES cinema(Id) ON DELETE CASCADE
            );
        ";
    }
}