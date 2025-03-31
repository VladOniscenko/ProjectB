using Dapper;
using ProjectB.Database;

namespace ProjectB.Models.Users;

public class UserRepository
{
    public static void InitializeDatabase()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Reservations TEXT,
                IsAdmin INTEGER NOT NULL CHECK (IsAdmin IN (0,1))
            );
        ");
    }
    
    public void AddUser(User user)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO Users (FirstName, LastName, Email, Password, Reservations, IsAdmin) 
            VALUES (@FirstName, @LastName, @Email, @Password, @Reservations, @IsAdmin)", user);
    }

    public IEnumerable<User> GetAllUsers()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<User>("SELECT * FROM Users");
    }
}