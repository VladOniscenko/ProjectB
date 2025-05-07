using Dapper;
using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.DataAccess;

public class ReservationRepository
{
    public static void InitializeTable()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Reservations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                ShowtimeId INTEGER NOT NULL,
                CreationDate TEXT NOT NULL,
                Status TEXT NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled')),
                TotalPrice REAL NOT NULL DEFAULT 0.0,
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (ShowtimeId) REFERENCES Showtimes(Id)
            );
        ");
    }
    
    public int AddReservation(Reservation reservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
    
        var reservationId = connection.ExecuteScalar<int>(@"
        INSERT INTO Reservations (UserId, ShowtimeId, CreationDate, Status, TotalPrice) 
        VALUES (@UserId, @ShowtimeId, @CreationDate, @Status, @TotalPrice);
        SELECT last_insert_rowid();", reservation);

        return reservationId;
    }
    
    public bool UpdateReservation(Reservation reservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        var rowsAffected = connection.Execute(@"
        UPDATE Reservations 
        SET UserId = @UserId,
            ShowtimeId = @ShowtimeId,
            CreationDate = @CreationDate,
            Status = @Status,
            TotalPrice = @TotalPrice
        WHERE Id = @Id", reservation);

        return rowsAffected > 0;
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Reservation>("SELECT * FROM Reservations");
    }
    
    public void Delete(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            DELETE FROM Reservations WHERE Id = @Id", new { Id = id });
    }
}