using Dapper;
using ProjectB.Database;
using ProjectB.Models;
using SQLitePCL;

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

    public IEnumerable<Reservation> GetReservationsByUserID(User user)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        if (user == null || user.Id <= 0)
        {
            throw new ArgumentException("Invalid user or user ID.");
        }
        return connection.Query<Reservation>(@"SELECT * FROM Reservations", new { UserId = user.Id });
    }

    public void Cancel(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        connection.Execute(@"
            DELETE FROM Seatreservations 
            WHERE ReservationId = @Id", new { Id = id, });

        connection.Execute(@"
            UPDATE Reservations 
            SET Status = @Status 
            WHERE Id = @Id", new { Id = id, Status = "Cancelled" });
    }

    public Showtime GetShowtimeByShowtimeId(Reservation showtime)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Showtime>(@"SELECT * FROM Showtimes WHERE Id = @Id", new { Id = showtime.ShowtimeId }).First();
    }

    public Movie GetMovieByShowtimeId(Reservation reservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Movie>(@"SELECT * FROM Movies WHERE Id = @Id", new { GetShowtimeByShowtimeId(reservation).Id }).First();
    }

    public List<int> GetSeatIdByReservationId(Reservation reservation)
    {

        using var connection = DbFactory.CreateConnection();
        connection.Open();
        var seat = connection.Query<int>(@"SELECT SeatId FROM SeatReservations WHERE ReservationId = @Id", new { reservation.Id }).ToList();
        return seat;
    }

    public List<Tuple<int, int>> GetSeatsFromSeatReservation(List<int> seatReservationId)
    {
        List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        foreach (int number in seatReservationId)
        {
            var SeatInfo = connection.Query<Seat>(@"SELECT * FROM Seats WHERE Id = @Id", new { Id = number }).First();
            Tuple<int, int> tuple = Tuple.Create(SeatInfo.Row, SeatInfo.Number);
            tuples.Add(tuple);
        }

        return tuples;
    }

    public string GetAuditoriumInfoByShowtime(Showtime showtime)
    {

        using var connection = DbFactory.CreateConnection();
        connection.Open();
        var auditorium = connection.Query<string>(@"SELECT Name FROM Auditoriums WHERE Id = @Id", new { Id = showtime.AuditoriumId }).First();
        return auditorium;
    }
    public IEnumerable<Reservation> GetReservationsById(int userId)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Reservation>("SELECT * FROM Reservations WHERE UserId = @UserId",
            new { UserId = userId });
    }
}