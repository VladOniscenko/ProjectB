using Dapper;
using ProjectB.Database;
using System.Text.Json;
using ProjectB.Models;

namespace ProjectB.DataAccess;

public class MovieRepository
{
    public static void InitializeTable()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Movies (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NOT NULL,
                Runtime INTEGER NOT NULL,
                Actors TEXT NOT NULL,
                Rating REAL NOT NULL,
                Genre TEXT NOT NULL,
                AgeRestriction INT NOT NULL,
                ReleaseDate TEXT NOT NULL,
                Country TEXT NOT NULL
            );
        ");
    }

    public static void PopulateTable()
    {
        try
        {
            var movieRepo = new MovieRepository();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "movies.json");

            if (!File.Exists(filePath))
            {
                throw new Exception("movies.json file not found.");
            }

            string jsonContent = File.ReadAllText(filePath);
            List<Movie>? movies = JsonSerializer.Deserialize<List<Movie>>(jsonContent);

            if (movies == null || movies.Count == 0)
            {
                throw new Exception("No movies found in the JSON file.");
            }

            foreach (Movie movie in movies)
            {
                movieRepo.AddMovie(movie);
            }

            Console.WriteLine($"Added: {movies.Count} Movies");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing movies.json: {ex.Message}");
        }
    }


    public void AddMovie(Movie movie)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Movies (Title, Description, Runtime, Actors, Rating, Genre, AgeRestriction, ReleaseDate, Country) 
            VALUES (@Title, @Description, @Runtime, @Actors, @Rating, @Genre, @AgeRestriction, @ReleaseDate, @Country)",
            movie);
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Movie>("SELECT * FROM Movies");
    }
    
    public List<Movie> GetNewestMovies(int count = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies ORDER BY ReleaseDate DESC LIMIT @Count",
            new { Count = count }
        ).ToList();
    }
    
    public List<Movie> GetBestRatedMovies(int count = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies ORDER BY Rating DESC LIMIT @Count",
            new { Count = count }
        ).ToList();
    }
    
    public List<Movie> GetBestAndNewestMovies(int count = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies ORDER BY Rating DESC, ReleaseDate DESC LIMIT @Count",
            new { Count = count }
        ).ToList();
    }
    
    public List<Movie> GetMoviesByGenre(string genre, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Genre) LIKE LOWER('%' || @Genre || '%') LIMIT @Count",
            new { Count = limit, Genre = genre }
        ).ToList();
    }
    
    public List<Movie> GetMoviesByTitle(string title, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Title) LIKE LOWER('%' || @Title || '%') LIMIT @Count",
            new { Count = limit, Title = title }
        ).ToList();
    }

    public List<Movie> GetMoviesWithShowtimeInNextDays(int days, int limit = 50)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        return connection.Query<Movie>(
            "SELECT m.* FROM Movies as m LEFT JOIN Showtimes as s ON s.MovieId = m.Id WHERE s.StartTime BETWEEN DATETIME('now') AND DATETIME('now', '+' || @Days || ' days') GROUP BY m.Id LIMIT @Limit",
            new {Days = days, Limit = limit}
        ).ToList();
    }
    
    public Movie? Find(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        return connection.QuerySingleOrDefault<Movie>(
            "SELECT * FROM Movies WHERE Id = @Id",
            new { Id = id }
        );
    }
}