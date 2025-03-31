using Dapper;
using ProjectB.Database;

namespace ProjectB.Models.Auditoriums;

public class AuditoriumRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Auditoriums (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TotalSeats INTEGER NOT NULL,
                CinemaId INTEGER NOT NULL,
                FOREIGN KEY (CinemaId) REFERENCES Cinemas(Id)
            );
        ");
    }
    
    public void AddAuditorium(Auditorium auditorium)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Auditoriums (TotalSeats, CinemaId) 
            VALUES (@TotalSeats, @CinemaId)", auditorium);
    }

    public IEnumerable<Auditorium> GetAllAuditoriums()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Auditorium>("SELECT * FROM Auditoriums");
    }
}