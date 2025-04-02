using Dapper;
using ProjectB.Database;
using ProjectB.Models.SeatReservations;

namespace ProjectB.DataAccess;

public class SeatReservationRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS SeatReservations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SeatId INTEGER NOT NULL,
                ReservationId INTEGER NOT NULL,
                FOREIGN KEY (SeatId) REFERENCES Seats(Id)
            );
        ");
    }
    
    public void AddSeatReservation(SeatReservation seatReservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO SeatReservations (SeatId, ReservationId) 
            VALUES (@SeatId, @ReservationId)", seatReservation);
    }

    public IEnumerable<SeatReservation> GetAllSeatReservations()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<SeatReservation>("SELECT * FROM SeatReservations");
    }
}