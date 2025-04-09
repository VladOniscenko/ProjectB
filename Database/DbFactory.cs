using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;
using ProjectB.DataAccess;

namespace ProjectB.Database;

public class DbFactory
{
    private const string ConnectionString = "Data Source=cinema.db";
    
    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
    
    public static void InitializeDatabase(bool dropAndFill = false)
    {
        if (dropAndFill) {
            DbFactory.DropTables();
        }
        
        // create tables
        MovieRepository.InitializeTable();
        UserRepository.InitializeTable();
        CinemaRepository.InitializeTable();
        AuditoriumRepository.InitializeTable();
        ShowtimeRepository.InitializeTable();
        SeatReservationRepository.InitializeTable();
        SeatRepository.InitializeTable();
        ReservationRepository.InitializeTable();
        
        // check if database is filled
        if (!dropAndFill)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                int movieCount = connection.QuerySingle<int>("SELECT COUNT(*) FROM Movies;");
                dropAndFill = movieCount == 0;
            }
        }

        // populate database if it is empty
        if (dropAndFill) {
            DbFactory.PopulateDatabase();
        }
    }
    
    public static void ExecuteWithLogging(Action action, string processName)
    {
        Console.WriteLine();
        Console.WriteLine($"Started {processName}.");
    
        try
        {
            action();
            Console.WriteLine($"{processName} completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during {processName}: {ex.Message}");
        }
    }

    public static void DropTables()
    {
        using (var connection = CreateConnection())
        {
            connection.Open();
        
            var dropTablesQuery = @"
                PRAGMA foreign_keys = OFF;
                DROP TABLE IF EXISTS SeatReservations;
                DROP TABLE IF EXISTS Reservations;
                DROP TABLE IF EXISTS Seats;
                DROP TABLE IF EXISTS Showtimes;
                DROP TABLE IF EXISTS Auditoriums;
                DROP TABLE IF EXISTS Cinemas;
                DROP TABLE IF EXISTS Movies;
                DROP TABLE IF EXISTS Users;
                PRAGMA foreign_keys = ON;
            ";
        
            connection.Execute(dropTablesQuery);
        }
    }

    
    public static void PopulateDatabase()
    {
        // first create cinema
        ExecuteWithLogging(CinemaRepository.PopulateTable, "Adding Cinemas to the database");

        // create admin user
        ExecuteWithLogging(UserRepository.PopulateTable, "Adding Users to the database");
        
        // create auditoriums and seats
        ExecuteWithLogging(AuditoriumRepository.PopulateTable, "Adding Auditoriums to the database");
        ExecuteWithLogging(SeatRepository.PopulateTable, "Adding Seats to the database");

        // create movies and show times
        ExecuteWithLogging(MovieRepository.PopulateTable, "Adding Movies to the database");
        ExecuteWithLogging(ShowtimeRepository.PopulateTable, "Adding Show times to the database");
    }
}