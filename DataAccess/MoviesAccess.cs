using Dapper;
using Microsoft.Data.Sqlite;
using ProjectB.DataModels;
using Microsoft.Data.Sqlite;
using Dapper;


namespace ProjectB.DataAccess
{
    public static class MoviesAccess
    {
        private static readonly DbAccess<Movie> DbAccess = new DbAccess<Movie>("movie");

        public static void Write(Movie movie) => DbAccess.Write(movie);
        
        public static void Update(Movie movie) => DbAccess.Update(movie);

        public static void Delete(int id) => DbAccess.Delete(id);

        public static Movie GetByTitle(string title)
        {
            string sql = $"SELECT * FROM movie WHERE title = @Title LIMIT 1";
            
            using (var connection = new SqliteConnection(DbAccess.ConnectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Movie>(sql, new { Title = title });
            }
        }
    }
}