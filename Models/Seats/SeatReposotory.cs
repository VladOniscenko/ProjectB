using Dapper;
using ProjectB.Database;

namespace ProjectB.Models.Seats;

public class SeatRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Seats (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AuditoriumId INTEGER NOT NULL,
                Row TEXT NOT NULL,
                Number INTEGER NOT NULL,
                FOREIGN KEY (AuditoriumId) REFERENCES Auditoriums(Id)
            );
        ");
    }
    
    public void AddSeat(Seat seat)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Seats (AuditoriumId, Row, Number) 
            VALUES (@AuditoriumId, @Row, @Number)", seat);
    }

    public IEnumerable<Seat> GetAllSeats()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Seat>("SELECT * FROM Seats");
    }
}