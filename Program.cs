using System;
using Microsoft.Data.Sqlite;
using ProjectB.DataAccess;
using ProjectB.DataModels;
using ProjectB.Logic;

namespace ProjectB
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello");

            if (!TestDatabaseConnection())
            {
                Console.WriteLine("Database connection failed. Exiting program.");
                return;
            }
            var moviesLogic = new MoviesLogic();
            Movie createdMovie = moviesLogic.Create();

            Console.WriteLine($"Created Movie: {createdMovie.Title} ({createdMovie.ReleaseDate.Year})");
        }

        static bool TestDatabaseConnection()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source=DataSources/cinema.db"))
                {
                    connection.Open();
                    Console.WriteLine("Database connection successful!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}