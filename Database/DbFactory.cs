using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;
using ProjectB.Auditoriums;
using ProjectB.Cinemas;
using ProjectB.Movies;
using ProjectB.Reservations;
using ProjectB.SeatReservations;
using ProjectB.Seats;
using ProjectB.Showtimes;
using ProjectB.Users;

namespace ProjectB.Database;

public class DbFactory
{
    private const string ConnectionString = "Data Source=cinema.db";
    
    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
    
    public static void InitializeDatabase()
    {
        MovieRepository.InitializeDatabase();
        UserRepository.InitializeDatabase();
        CinemaRepository.InitializeDatabase();
        AuditoriumRepository.InitializeDatabase();
        ShowtimeRepository.InitializeDatabase();
        SeatReservationRepository.InitializeDatabase();
        SeatRepository.InitializeDatabase();
        ReservationRepository.InitializeDatabase();
    }
}