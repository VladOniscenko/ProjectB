using Microsoft.Data.Sqlite;
using ProjectB.DataModels;

namespace ProjectB.DataAccess;

public static class DbInitializer
{
    public static readonly string Dir = "DataSources";
    public static readonly string Db = "cinema.db";
    public static readonly string DbPath = $"{Dir}/{Db}";
    
    public static readonly string ConnectionString = $"Data Source={DbPath}";
    
    public static void Initialize()
    {
        EnsureDatabaseExists();
        
        try
        {
            using (var connection = new SqliteConnection($"{DbInitializer.ConnectionString}"))
            {
                connection.Open();

                string sql = "";
                sql += Movie.Table();
                sql += Auditorium.Table();
                sql += User.Table();
                sql += Cinema.Table();

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database-init error: {ex.Message}");
        }
    }
    
    public static void EnsureDatabaseExists()
    {
        string directoryPath = Path.GetDirectoryName(DbPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Console.WriteLine($"Created missing directory: {directoryPath}");
        }

        if (!File.Exists(DbPath))
        {
            File.Create(DbPath).Close();
            Console.WriteLine($"Database file not found, created a new one at: {DbPath}");
        }
    }
}