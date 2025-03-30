using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace CinemaApp.DataAccess;

public static class DbInitializer
{
    public static void Initialize()
    {
        try
        {
            using (var connection = new SqliteConnection($"Data Source={DbInitializer.GetDbPath()}"))
            {
                connection.Open();

                string sql = @"
                CREATE TABLE IF NOT EXISTS movie (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Runtime INTEGER,
                    Actors TEXT,
                    Rating INTEGER,
                    Genre TEXT,
                    AgeRestriction INTEGER,
                    ReleaseDate TEXT,
                    Country TEXT
                );";

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


    public static string GetDbPath()
    {
        return "cinema.db";
    }

    
    public static void RemoveDbFile()
    {
        File.Delete(DbInitializer.GetDbPath());
    }

}