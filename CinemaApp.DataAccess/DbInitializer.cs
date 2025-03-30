using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace CinemaApp.DataAccess;

public static class DbInitializer
{
    public static void Initialize()
    {
        string connectionString = DbInitializer.GetDbPath();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS movie (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    description TEXT,
                    runtime INTEGER,
                    actors TEXT,
                    rating REAL,
                    genre TEXT,
                    AgeRestriction INTEGER,
                    release_date TEXT,
                    country TEXT
                );";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static string GetDbPath()
    {
        return "Data Source=cinema.db;";
    }
}