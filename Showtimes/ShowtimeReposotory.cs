using Dapper;
using ProjectB.Database;

namespace ProjectB.Showtimes;

public class ShowtimeRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Showtimes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                MovieId INTEGER NOT NULL,
                AuditoriumId INTEGER NOT NULL,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                FOREIGN KEY (MovieId) REFERENCES Movies(Id),
                FOREIGN KEY (AuditoriumId) REFERENCES Auditoriums(Id)
            );
        ");
    }
    
    public void AddShowtime(Showtime showtime)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Showtimes (MovieId, AuditoriumId, StartTime, EndTime) 
            VALUES (@MovieId, @AuditoriumId, @StartTime, @EndTime)", showtime);
    }

    public IEnumerable<Showtime> GetAllShowtimes()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Showtime>("SELECT * FROM Showtimes");
    }
}