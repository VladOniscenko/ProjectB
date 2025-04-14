using Dapper;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Models;

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
                Active  INT NULL DEFAULT 0,
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

        foreach (Auditorium auditorium in auditoriumRepository.GetAllAuditoriums())
        {
            // Uitgeschakelde stoelen per auditorium, expliciet gedefinieerd
            HashSet<(int row, int number)> disabledSeats = new();

            if (auditorium.Id == 1)
            {
                disabledSeats = new()
                {
                    (1, 1), (1, 2), (1, 3), (1, 12), (1, 13), (1, 14),
                    (2, 1), (2, 2), (2, 14),
                    (11, 1), (11, 2), (11, 14),
                    (12, 1), (12, 2), (12, 3), (12, 12), (12, 13), (12, 14)
                };
            }
            else if (auditorium.Id == 2)
            {
                disabledSeats = new()
                {
                    // col 1
                    (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1),
                    (14, 1), (15, 1), (16, 1), (17, 1), (18, 1), (19, 1),
                    // col 2
                    (1, 2), (2, 2), (3, 2), (4, 2), (5, 2),
                    // col 3
                    (1, 3), (2, 3),
                    // col 16
                    (1, 16), (2, 16),
                    // col 17
                    (1, 17), (2, 17), (3, 17), (4, 17), (5, 17),
                    // col 18
                    (1, 18), (2, 18), (3, 18), (4, 18), (5, 18), (6, 18), (7, 18), (8, 18),
                    (14, 18), (15, 18), (16, 18), (17, 18), (18, 18), (19, 18)
                };
            }
            else if (auditorium.Id == 3)
            {
                disabledSeats = new()
                {
                    (1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (1, 8), (1, 14), (1, 15), (1, 16), (1, 17),
                    (1, 18), (1, 19), (1, 20),
                    (2, 1), (2, 2), (2, 3), (2, 4), (2, 5), (2, 6), (2, 7), (2, 15), (2, 16), (2, 17), (2, 18), (2, 19),
                    (2, 20),
                    (3, 1), (3, 2), (3, 3), (3, 4), (3, 5), (3, 16), (3, 17), (3, 18), (3, 19), (3, 20),
                    (4, 1), (4, 2), (4, 3), (4, 20),
                    (5, 1), (5, 2), (5, 3),
                    (6, 1), (6, 2),
                    (7, 1), (7, 2),
                    (8, 1),
                    (23, 1),
                    (24, 1), (24, 2),
                    (25, 1), (25, 2),
                    (26, 1), (26, 2), (26, 3),
                    (27, 1), (27, 2), (27, 3), (27, 20),
                    (28, 1), (28, 2), (28, 3), (28, 4), (28, 5), (28, 16), (28, 17), (28, 18), (28, 19), (28, 20),
                    (29, 1), (29, 2), (29, 3), (29, 4), (29, 5), (29, 6), (29, 7), (29, 15), (29, 16), (29, 17),
                    (29, 18), (29, 19), (29, 20),
                    (30, 1), (30, 2), (30, 3), (30, 4), (30, 5), (30, 6), (30, 7), (30, 8), (30, 14), (30, 15),
                    (30, 16), (30, 17), (30, 18), (30, 19), (30, 20)
                };
            }

            int maxSeats = auditorium.TotalSeats;
            int seatsPerRow = 10;
            int rows = (int)Math.Ceiling((double)maxSeats / seatsPerRow);

            int seatNumber = 1;
            for (int row = 1; row <= rows; row++)
            {
                for (int number = 1; number <= seatsPerRow; number++)
                {
                    if (seatNumber > maxSeats)
                        break;

                    Seat seat = new Seat
                    {
                        AuditoriumId = auditorium.Id,
                        Row = row,
                        Number = number,
                        Active = !disabledSeats.Contains((row, number))
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

    public IEnumerable<Seat> GetSeatsByAuditorium(int id)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Seat>(@"SELECT * FROM Seats WHERE AuditoriumId = @Id ORDER BY Row", new { Id = id });
    }
}