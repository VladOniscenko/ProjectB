using Dapper;
using ProjectB.Database;
using BCrypt.Net;
using ProjectB.Models;
using System.Windows.Markup;

namespace ProjectB.DataAccess;

public class UserRepository
{
    public static void InitializeTable()
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
                IsAdmin INTEGER NOT NULL CHECK (IsAdmin IN (0,1))
            );
        ");
    }

    public static void PopulateTable()
    {
        try
        {
            var userRepo = new UserRepository();

            // Check if an admin with the specific email exists
            if (!userRepo.CheckIfUserExistByEmail("admin"))
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin",
                    Password = "admin",
                    IsAdmin = true
                };

                userRepo.AddUser(adminUser);
                Console.WriteLine("Admin user created.");
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing admin user: {ex.Message}");
        }
    }

    public int? AddUser(User user)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        // Hash the password before storing it
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return connection.QuerySingle<int>(@"INSERT INTO Users (FirstName, LastName, Email, Password, IsAdmin) VALUES (@FirstName, @LastName, @Email, @Password, @IsAdmin); SELECT last_insert_rowid();", user);
    }

    public List<User> GetAllUsers()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<User>("SELECT * FROM Users").ToList();
    }

    public List<User> GetAllNonAdminUsers(){
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<User>("SELECT * FROM Users WHERE IsAdmin = 0").ToList();
    }

    public bool CheckIfUserExistByEmail(string email)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return !(connection.Query<User>("SELECT * FROM Users WHERE Email = @email", new { email }).Count() == 0);
    }

    public User? GetUserByEmail(string email)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Email = @email LIMIT 1", new { email });
    }

    public void MakeUserAdmin(string email){
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        connection.Execute(@"
        UPDATE Users SET IsAdmin = 1 WHERE Email = @email",new { email });
    }
    // Gotta thank Jesse for the help on this one
    public void UpdateUser(User user)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        string query = @"
            UPDATE Users
            SET FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                Password = @Password,
                IsAdmin = @IsAdmin
            WHERE Id = @Id";

        connection.Execute(query, new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Password,
            user.IsAdmin,
            user.Id
        });
    }

    public User? GetUserByReservationId(Reservation reservation)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Id = @UserId LIMIT 1", new { reservation.UserId });
    }
}