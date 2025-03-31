using Dapper;
using ProjectB.Database;

namespace ProjectB.Models.Movies;

public class MovieRepository
{
    public static void InitializeDatabase()
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
                AgeRestriction TEXT NOT NULL,
                ReleaseDate TEXT NOT NULL,
                Country TEXT NOT NULL
            );
        ");
    }
    
    public void AddMovie(Movie movie)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Movies (Title, Description, Runtime, Actors, Rating, Genre, AgeRestriction, ReleaseDate, Country) 
            VALUES (@Title, @Description, @Runtime, @Actors, @Rating, @Genre, @AgeRestriction, @ReleaseDate, @Country)", movie);
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Movie>("SELECT * FROM Movies");
    }
}