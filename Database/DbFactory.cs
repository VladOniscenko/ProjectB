using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;
using ProjectB.Models.Auditoriums;
using ProjectB.Models.Cinemas;
using ProjectB.Models.Movies;
using ProjectB.Models.Reservations;
using ProjectB.Models.SeatReservations;
using ProjectB.Models.Seats;
using ProjectB.Models.Showtimes;
using ProjectB.Models.Users;

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