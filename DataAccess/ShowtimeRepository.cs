using Dapper;
using ProjectB.Database;
using ProjectB.Models.Showtimes;

namespace ProjectB.DataAccess;

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
    
    public void DeleteAllShowtimes()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute("DELETE FROM Showtimes");
    }
    
public static void PopulateTable()
{
    MovieRepository movieRepository = new();
    AuditoriumRepository auditoriumRepository = new();
    ShowtimeRepository showtimeRepository = new();

    var movies = movieRepository.GetBestAndNewestMovies(25);
    var auditoriums = auditoriumRepository.GetAllAuditoriums();

    if (!movies.Any() || !auditoriums.Any())
    {
        Console.WriteLine("No movies or auditoriums found.");
        return;
    }

    showtimeRepository.DeleteAllShowtimes();

    DateTime startOfWeek = DateTime.Now.Date;
    Random random = new();

    foreach (var movie in movies)
    {
        foreach (var auditorium in auditoriums)
        {
            List<DateTime> bookedTimes = new();

            for (int i = 0; i < 10; i++)
            {
                DateTime startTime;
                DateTime endTime;
                bool overlap;

                do
                {
                    startTime = startOfWeek.AddDays(random.Next(7))
                        .AddHours(random.Next(9, 22))
                        .AddMinutes(15 * random.Next(0, 4));

                    endTime = startTime.AddMinutes(movie.Runtime + random.Next(15, 30)); // Add buffer

                    overlap = bookedTimes.Any(existing => 
                        (startTime >= existing && startTime < existing.AddMinutes(movie.Runtime + 15)));

                } while (overlap);

                bookedTimes.Add(startTime);

                Showtime showtime = new()
                {
                    MovieId = movie.Id,
                    AuditoriumId = auditorium.Id,
                    StartTime = startTime,
                    EndTime = endTime
                };

                showtimeRepository.AddShowtime(showtime);
            }

            Console.WriteLine($"Showtimes created for movie: {movie.Title} in Auditorium {auditorium.Id}");
        }
    }
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