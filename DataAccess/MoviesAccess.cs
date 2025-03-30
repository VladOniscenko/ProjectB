using Microsoft.Data.Sqlite;
using Dapper;

using ProjectB.DataModels;
namespace ProjectB.DataAccess;

public class MoviesAccess : DbAccess<Movie>
{
    private const string TableName = "movie";

    public MoviesAccess() : base(TableName) { }

    public Movie GetByTitle(string title)
    {
        string sql = $"SELECT * FROM movie WHERE title = @Title";

        using (var connection = new SqliteConnection($"Data Source={DbInitializer.GetDbPath()}"))
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Movie>(sql, new { Title = title });
        }
    }
}