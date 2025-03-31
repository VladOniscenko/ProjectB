using Dapper;
using ProjectB.Database;

namespace ProjectB.Models.Cinemas;

public class CinemaRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Cinemas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CinemaName TEXT NOT NULL
            );
        ");
    }
    
    public void AddCinema(Cinema cinema)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Cinemas (CinemaName) 
            VALUES (@CinemaName)", cinema);
    }

    public IEnumerable<Cinema> GetAllCinemas()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Cinema>("SELECT * FROM Cinemas");
    }
}