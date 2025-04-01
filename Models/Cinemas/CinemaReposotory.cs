using Dapper;
using ProjectB.Database;
using ProjectB;
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
    
    public static void PopulateTable()
    {
        string cinemaName = Cinema.MainCinemaName;
        CinemaRepository cinemaRepo = new();
        if (!cinemaRepo.GetAllCinemas().Any(u => u.CinemaName == cinemaName))
        {
            cinemaRepo.AddCinema(new Cinema(cinemaName));
        }
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

    public Cinema? GetMainCinema()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        var cinema = connection.QueryFirstOrDefault<Cinema>(
            "SELECT * FROM Cinemas WHERE CinemaName = @CinemaName", 
            new { CinemaName = Cinema.MainCinemaName });

        return cinema;
    }
}