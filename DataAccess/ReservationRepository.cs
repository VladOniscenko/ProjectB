using Dapper;
using ProjectB.Database;
using ProjectB.Models.Reservations;

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
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (ShowtimeId) REFERENCES Showtimes(Id)
            );
        ");
    }
    
    public void AddReservation(Reservation reservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Reservations (UserId, ShowtimeId, CreationDate) 
            VALUES (@UserId, @ShowtimeId, @CreationDate)", reservation);
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Reservation>("SELECT * FROM Reservations");
    }
}