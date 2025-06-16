using Dapper;
using ProjectB.Database;
using System.Text.Json;
using ProjectB.Models;
using Bogus; // used to generate fake actor names for the movies

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
        var faker = new Faker(); 

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
                string names = "";
                for (int i = 0; i < 5; i++) 
                { 
                    names += faker.Name.FirstName() + " " + faker.Name.LastName() + ",";
                }
                movie.Actors = names;
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

    public List<Movie> GetMoviesByActor(string actor, int limit){
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Actors) LIKE LOWER('%' || @Actor || '%') LIMIT @Count",
            new { Count = limit, Actor = actor }
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
    
    public List<Movie> SearchMoviesByFilters(string title, string genre, string actor, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        string query = "SELECT * FROM Movies";
        bool where = false;

        if (!string.IsNullOrWhiteSpace(title))
        {
            query += where ? " AND" : " WHERE";
            query += " LOWER(Title) LIKE LOWER('%' || @Title || '%')";
            where = true;
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query += where ? " AND" : " WHERE";
            query += " LOWER(Genre) LIKE LOWER('%' || @Genre || '%')";
            where = true;
        }

        if (!string.IsNullOrWhiteSpace(actor))
        {
            query += where ? " AND" : " WHERE";
            query += " LOWER(Actors) LIKE LOWER('%' || @Actor || '%')";
            where = true;
        }

        query += " LIMIT @Count";
        return connection.Query<Movie>(query, new { Title = title, Genre = genre, Actor = actor, Count = limit }).ToList();
    }

    public List<Movie> GetMoviesByTitleAndGenre(string title, string genre, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Title) LIKE LOWER('%' || @Title || '%')" +
            "AND LOWER(Genre) LIKE LOWER('%' || @Genre || '%') LIMIT @Count",
            new { Count = limit, Title = title, Genre = genre }
        ).ToList();
    }

    public List<Movie> GetMoviesByTitleAndActor(string title, string actor, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Title) LIKE LOWER('%' || @Title || '%')" +
            "AND LOWER(Actors) LIKE LOWER('%' || @Actor || '%') LIMIT @Count",
            new { Count = limit, Title = title, Actor = actor }
        ).ToList();
    }

    public List<Movie> GetMoviesByGenreAndActor(string genre, string actor, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Genre) LIKE LOWER('%' || @Genre || '%')" +
            "AND LOWER(Actors) LIKE LOWER('%' || @Actor || '%') LIMIT @Count",
            new { Count = limit, Genre = genre, Actor = actor }
        ).ToList();
    }

    public List<Movie> GetMoviesByTitleGenreAndActor(string title, string genre, string actor, int limit = 10)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        return connection.Query<Movie>(
            "SELECT * FROM Movies WHERE LOWER(Title) LIKE LOWER('%' || @Title || '%')" +
            "AND LOWER(Genre) LIKE LOWER('%' || @Genre || '%')" +
            "AND LOWER(Actors) LIKE LOWER('%' || @Actor || '%') LIMIT @Count",
            new { Count = limit, Title = title, Genre = genre , Actor = actor}
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

    public List<string> GetGenre(string genre)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        return connection.Query<string>(
            "SELECT * FROM Movies WHERE LOWER(Genre) LIKE LOWER('%' || @Genre || '%') LIMIT @Count",
            new { Count = 1, Genre = genre }
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