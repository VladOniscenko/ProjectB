using Dapper;
using ProjectB.Database;
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
                Active  INT NULL,
                Type TEXT NULL,
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
            HashSet<(int row, int number)> disabledSeats = new();
            HashSet<(int row, int number)> premiumSeats = new();
            HashSet<(int row, int number)> vipSeats = new();
            int rows = 0;
            int cols = 0;

            if (auditorium.Id == 1)
            {
                disabledSeats = new()
                {
                    (1, 1), (1, 2), (1, 11), (1, 12),
                    (2, 1), (2, 2), (2, 11), (2, 12),
                    (3, 1), (3, 12),
                    (12, 1), (12, 12),
                    (13, 1), (13, 12),
                    (14, 1), (14, 2), (14, 11), (14, 12)
                };

                premiumSeats = new()
                {
                    (4, 6), (4, 7),
                    (5, 5), (5, 6), (5, 7), (5, 8),
                    (6, 4), (6, 5), (6, 8), (6, 9),
                    (7, 4), (7, 5), (7, 8), (7, 9),
                    (8, 4), (8, 5), (8, 8), (8, 9),
                    (9, 4), (9, 5), (9, 8), (9, 9),
                    (10, 5), (10, 6), (10, 7), (10, 8),
                    (11, 6), (11, 7)
                };

                vipSeats = new()
                {
                    (6, 6), (6, 7),
                    (7, 6), (7, 7),
                    (8, 6), (8, 7),
                    (9, 6), (9, 7)
                };

                rows = 14;
                cols = 12;
            }
            else if (auditorium.Id == 2)
            {
                disabledSeats = new()
                {
                    (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1),
                    (14, 1), (15, 1), (16, 1), (17, 1), (18, 1), (19, 1),
                    (1, 2), (2, 2), (3, 2), (4, 2), (5, 2),
                    (1, 3), (2, 3),
                    (1, 16), (2, 16),
                    (1, 17), (2, 17), (3, 17), (4, 17), (5, 17),
                    (1, 18), (2, 18), (3, 18), (4, 18), (5, 18), (6, 18), (7, 18), (8, 18),
                    (14, 18), (15, 18), (16, 18), (17, 18), (18, 18), (19, 18)
                };
                
                premiumSeats = new()
                {
                    (4, 7), (4, 8), (4, 9), (4, 10), (4, 11), (4, 12),
                    (5, 7), (5, 8), (5, 9), (5, 10), (5, 11), (5, 12),
                    (6, 6), (6, 7), (6, 8), (6, 9), (6, 10), (6, 11), (6, 12), (6, 13),
                    (7, 5), (7, 6), (7, 7), (7, 8), (7, 11), (7, 12), (7, 13), (7, 14),
                    (8, 4), (8, 5), (8, 6), (8, 7), (8, 12), (8, 13), (8, 14), (8, 15),
                    (9, 3), (9, 4), (9, 5), (9, 6), (9, 13), (9, 14), (9, 15), (9, 16),
                    (10, 3), (10, 4), (10, 5), (10, 6), (10, 13), (10, 14), (10, 15), (10, 16),
                    (11, 3), (11, 4), (11, 5), (11, 6), (11, 13), (11, 14), (11, 15), (11, 16),
                    (12, 4), (12, 5), (12, 6), (12, 13), (12, 14), (12, 15),
                    (13, 4), (13, 5), (13, 6), (13, 7), (13, 12), (13, 13), (13, 14), (13, 15),
                    (14, 5), (14, 6), (14, 7), (14, 8), (14, 11), (14, 12), (14, 13), (14, 14),
                    (15, 5), (15, 6), (15, 7), (15, 8), (15, 9), (15, 10), (15, 11), (15, 12), (15, 13), (15, 14),
                    (16, 6), (16, 7), (16, 8), (16, 9), (16, 10), (16, 11), (16, 12), (16, 13), 
                    (16, 6), (17, 7), (17, 8), (17, 9), (17, 10), (17, 11), (17, 12), (17, 13), 
                    (18, 7), (18, 8), (18, 9), (18, 10), (18, 11), (18, 12),
                };
                
                vipSeats = new()
                {
                    (7, 9), (7, 10),
                    (8, 8), (8, 9), (8, 10), (8, 11),
                    (9, 7), (9, 8), (9, 9), (9, 10), (9, 11), (9, 12), 
                    (10, 7), (10, 8), (10, 9), (10, 10), (10, 11), (10, 12), 
                    (11, 7), (11, 8), (11, 9), (11, 10), (11, 11), (11, 12), 
                    (12, 7), (12, 8), (12, 9), (12, 10), (12, 11), (12, 12), 
                    (13, 8), (13, 9), (13, 10), (13, 11),
                    (14, 9), (14, 10),
                };
                
                rows = 19;
                cols = 18;
            }
            else if (auditorium.Id == 3)
            {
                disabledSeats = new ()
                {
                    (1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (1, 8), (1, 23), (1, 24), (1, 25), (1, 26), (1, 27), (1, 28), (1, 29), (1, 30),
                    (2, 1), (2, 2), (2, 3), (2, 4), (2, 5), (2, 6), (2, 7), (2, 24), (2, 25), (2, 26), (2, 27), (2, 28), (2, 29), (2, 30),
                    (3, 1), (3, 2), (3, 3), (3, 4), (3, 5), (3, 26), (3, 27), (3, 28), (3, 29), (3, 30),
                    (4, 1), (4, 2), (4, 3), (4, 28), (4, 29), (4, 30),
                    (5, 1), (5, 2), (5, 29), (5, 30),
                    (6, 1), (6, 30),
                    (14, 1), (14, 30),
                    (15, 1), (15, 2), (15, 29), (15, 30),
                    (16, 1), (16, 2), (16, 3), (16, 28), (16, 29), (16, 30),
                    (17, 1), (17, 2), (17, 3), (17, 28), (17, 29), (17, 30),
                    (18, 1), (18, 2), (18, 3), (18, 28), (18, 29), (18, 30),
                    (19, 1), (19, 2), (19, 3), (19, 28), (19, 29), (19, 30),
                    (20, 1), (20, 2), (20, 3), (20, 4), (20, 27), (20, 28), (20, 29), (20, 30)
                };
                
                
                premiumSeats = new ()
                {
                    (4, 13), (4, 14), (4, 15), (4, 16), (4, 17), (4, 18),
                    (5, 11), (5, 12), (5, 13), (5, 14), (5, 15), (5, 16), (5, 17), (5, 18), (5, 19), (5, 20),
                    (6, 10), (6, 11), (6, 12), (6, 13), (6, 14), (6, 15), (6, 16), (6, 17), (6, 18), (6, 19), (6, 20), (6, 21),
                    (7, 9), (7, 10), (7, 11), (7, 12), (7, 13), (7, 14), (7, 15), (7, 16), (7, 17), (7, 18), (7, 19), (7, 20), (7, 21), (7, 22),
                    (8, 9), (8, 10), (8, 11), (8, 12), (8, 13), (8, 18), (8, 19), (8, 20), (8, 21), (8, 22),
                    (9, 8), (9, 9), (9, 10), (9, 11), (9, 20), (9, 21), (9, 22), (9, 23),
                    (10, 7), (10, 8), (10, 9), (10, 10), (10, 11), (10, 20), (10, 21), (10, 22), (10, 23), (10, 24),
                    (11, 6), (11, 7), (11, 8), (11, 9), (11, 10), (11, 11), (11, 20), (11, 21), (11, 22), (11, 23), (11, 24), (11, 25),
                    (12, 6), (12, 7), (12, 8), (12, 9), (12, 10), (12, 11), (12, 20), (12, 21), (12, 22), (12, 23), (12, 24), (12, 25),
                    (13, 7), (13, 8), (13, 9), (13, 10), (13, 11), (13, 20), (13, 21), (13, 22), (13, 23), (13, 24),
                    (14, 7), (14, 8), (14, 9), (14, 10), (14, 11), (14, 20), (14, 21), (14, 22), (14, 23), (14, 24),
                    (15, 8), (15, 9), (15, 10), (15, 11), (15, 12), (15, 19), (15, 20), (15, 21), (15, 22), (15, 23),
                    (16, 8), (16, 9), (16, 10), (16, 11), (16, 12), (16, 13), (16, 18), (16, 19), (16, 20), (16, 21), (16, 22), (16, 23),
                    (17, 9), (17, 10), (17, 11), (17, 12), (17, 13), (17, 14), (17, 15), (17, 16), (17, 17), (17, 18), (17, 19), (17, 20), (17, 21), (17, 22),
                    (18, 9), (18, 10), (18, 11), (18, 12), (18, 13), (18, 14), (18, 15), (18, 16), (18, 17), (18, 18), (18, 19), (18, 20), (18, 21), (18, 22),
                    (19, 10), (19, 11), (19, 12), (19, 13), (19, 14), (19, 15), (19, 16), (19, 17), (19, 18), (19, 19), (19, 20), (19, 21)
                };
                
                vipSeats = new ()
                {
                    (8, 14), (8, 15), (8, 16), (8, 17),
                    (9, 12), (9, 13), (9, 14), (9, 15), (9, 16), (9, 17), (9, 18), (9, 19),
                    
                    (10, 12), (10, 13), (10, 14), (10, 15), (10, 16), (10, 17), (10, 18), (10, 19),
                    (11, 12), (11, 13), (11, 14), (11, 15), (11, 16), (11, 17), (11, 18), (11, 19),
                    (12, 12), (12, 13), (12, 14), (12, 15), (12, 16), (12, 17), (12, 18), (12, 19),
                    (13, 12), (13, 13), (13, 14), (13, 15), (13, 16), (13, 17), (13, 18), (13, 19),
                    (14, 12), (14, 13), (14, 14), (14, 15), (14, 16), (14, 17), (14, 18), (14, 19),
                    (15, 13), (15, 14), (15, 15), (15, 16), (15, 17), (15, 18),
                    (16, 14), (16, 15), (16, 16), (16, 17)
                };

                rows = 20;
                cols = 30;
            }

            for (int row = 1; row <= rows; row++)
            {
                for (int number = 1; number <= cols; number++)
                {
                    bool isDisabled = disabledSeats.Contains((row, number));
                    string type = "disabled";

                    if (!isDisabled)
                    {
                        if (vipSeats.Contains((row, number)))
                        {
                            type = "vip";
                        }
                        else if (premiumSeats.Contains((row, number)))
                        {
                            type = "premium";
                        }
                        else
                        {
                            type = "normal";
                        }
                    }

                    Seat seat = new Seat
                    {
                        AuditoriumId = auditorium.Id,
                        Row = row,
                        Number = number,
                        Active = isDisabled ? 0 : 1,
                        Type = type
                    };

                    seatRepository.AddSeat(seat);
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
            INSERT INTO Seats (AuditoriumId, Row, Number, Active, Type) 
            VALUES (@AuditoriumId, @Row, @Number, @Active, @Type)", seat);
    }

    public IEnumerable<Seat> GetAllSeats()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Seat>("SELECT * FROM Seats");
    }

    public IEnumerable<Seat> GetSeatsByShowtime(int showtimeId)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<Seat>(@"
            WITH TargetShowtime AS (
                SELECT Id AS ShowtimeId, AuditoriumId
                FROM Showtimes
                WHERE Id = @Id
            )
            SELECT
                s.*,
                CASE
                    WHEN sr.SeatId IS NOT NULL THEN 1
                    ELSE 0
                    END AS Taken
            FROM Seats AS s
                     JOIN TargetShowtime AS ts ON s.AuditoriumId = ts.AuditoriumId
                     LEFT JOIN SeatReservations AS sr
                               ON sr.ShowtimeId = ts.ShowtimeId AND sr.SeatId = s.Id
            ORDER BY s.Row DESC, s.Number ASC;
        ",
            new { Id = showtimeId });
    }
}