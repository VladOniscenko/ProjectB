using Dapper;
using ProjectB.Database;
using ProjectB.Models.Auditoriums;
using ProjectB.Models.Seats;

namespace ProjectB.DataAccess;

public class SeatRepository
{
    public static void InitializeTable()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Seats (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AuditoriumId INTEGER NOT NULL,
                Row INTEGER NOT NULL,
                Number INTEGER NOT NULL,
                FOREIGN KEY (AuditoriumId) REFERENCES Auditoriums(Id)
            );
        ");
    }
    
    public void DeleteAllSeats()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute("DELETE FROM Seats");
    }
    
    public static void PopulateTable()
    {
        AuditoriumRepository auditoriumRepository = new();
        SeatRepository seatRepository = new();

        seatRepository.DeleteAllSeats();

        foreach (Auditorium auditorium in auditoriumRepository.GetAllAuditoriums()) {
            int maxSeats = auditorium.TotalSeats;
            int seatsPerRow = 10;
            int rows = (int)Math.Ceiling((double)maxSeats / seatsPerRow);

            int seatNumber = 1;
            for (int row = 1; row <= rows; row++) {
                for (int number = 1; number <= seatsPerRow; number++) {
                    if (seatNumber > maxSeats) {
                        break;
                    }

                    Seat seat = new Seat
                    {
                        AuditoriumId = auditorium.Id,
                        Row = row,
                        Number = number
                    };

                    seatRepository.AddSeat(seat);
                    seatNumber++;
                }
            }

            Console.WriteLine($"Seats created for Auditorium {auditorium.Name}");
        }
    }
    
    public void AddSeat(Seat seat)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Seats (AuditoriumId, Row, Number) 
            VALUES (@AuditoriumId, @Row, @Number)", seat);
    }

    public IEnumerable<Seat> GetAllSeats()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Seat>("SELECT * FROM Seats");
    }
}