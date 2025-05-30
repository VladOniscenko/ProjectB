using Dapper;
using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.DataAccess;

public class AuditoriumRepository
{
    public static void InitializeTable()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Auditoriums (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TotalSeats INTEGER NOT NULL,
                CinemaId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                FOREIGN KEY (CinemaId) REFERENCES Cinemas(Id)
            );
        ");
    }
    
    public static void PopulateTable()
    {
        CinemaRepository cinemaRepository = new();
        Cinema? cinema = cinemaRepository.GetMainCinema();

        if (cinema == null)
        {
            Console.WriteLine("Main cinema not found.");
            return;
        }

        AuditoriumRepository auditoriumRepository = new();

        // Check if the auditoriums already exist
        if (!auditoriumRepository.Exists("Auditorium 1", cinema.Id))
        {
            Auditorium auditorium1 = new Auditorium("Auditorium 1", 168, cinema.Id);
            auditoriumRepository.AddAuditorium(auditorium1);
            Console.WriteLine("Auditorium 1 created.");
        }
        
        if (!auditoriumRepository.Exists("Auditorium 2", cinema.Id))
        {
            Auditorium auditorium2 = new Auditorium("Auditorium 2", 342, cinema.Id);
            auditoriumRepository.AddAuditorium(auditorium2);
            Console.WriteLine("Auditorium 2 created.");
        }

        if (!auditoriumRepository.Exists("Auditorium 3", cinema.Id))
        {
            Auditorium auditorium3 = new Auditorium("Auditorium 3", 600, cinema.Id);
            auditoriumRepository.AddAuditorium(auditorium3);
            Console.WriteLine("Auditorium 3 created.");
        }
    }
    
    public bool Exists(string auditoriumName, int cinemaId)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        var count = connection.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM Auditoriums WHERE Name = @Name AND CinemaId = @CinemaId", 
            new { Name = auditoriumName, CinemaId = cinemaId });

        return count > 0;
    }
    
    public void AddAuditorium(Auditorium auditorium)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Auditoriums (TotalSeats, CinemaId, Name) 
            VALUES (@TotalSeats, @CinemaId, @Name)", auditorium);
    }

    public IEnumerable<Auditorium> GetAllAuditoriums()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Auditorium>("SELECT * FROM Auditoriums");
    }
    
    public Auditorium? Find(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        return connection.QuerySingleOrDefault<Auditorium>(
            "SELECT * FROM Auditoriums WHERE Id = @Id",
            new { Id = id }
        );
    }

    public bool IsAuditoriumTakenAt(int auditoriumId, DateTime startTime, DateTime endTime)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        
        const string sql = @"
        SELECT COUNT(*) 
        FROM Showtimes 
        WHERE AuditoriumId = @AuditoriumId 
        AND (
            (@StartTime BETWEEN StartTime AND EndTime) OR
            (@EndTime BETWEEN StartTime AND EndTime) OR
            (StartTime BETWEEN @StartTime AND @EndTime) OR
            (EndTime BETWEEN @StartTime AND @EndTime)
        )";
    
        var count = connection.ExecuteScalar<int>(sql, new 
        { 
            AuditoriumId = auditoriumId,
            StartTime = startTime,
            EndTime = endTime
        });
    
        return count > 0;
    }
}