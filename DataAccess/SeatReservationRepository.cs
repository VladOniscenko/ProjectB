using Dapper;
using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.DataAccess;

public class SeatReservationRepository
{
    public static void InitializeTable()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS SeatReservations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SeatId INTEGER NOT NULL,
                ReservationId INTEGER NOT NULL,
                ShowtimeId INTEGER NOT NULL,
                FOREIGN KEY (SeatId) REFERENCES Seats(Id)
            );
        ");
    }
    
    public void AddSeatReservation(SeatReservation seatReservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO SeatReservations (SeatId, ReservationId, ShowtimeId) 
            VALUES (@SeatId, @ReservationId, @ShowtimeId)", seatReservation);
    }

    public IEnumerable<SeatReservation> GetAllSeatReservations()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<SeatReservation>("SELECT * FROM SeatReservations");
    }
    
    public IEnumerable<SeatReservation> GetReservedSeatsByShowtimeId(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<SeatReservation>(@"SELECT * FROM SeatReservations WHERE ShowtimeId = @Id",
            new { Id = id });
    }
}