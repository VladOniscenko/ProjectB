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
            if (CheckIfUserExistByEmail("admin@admin.com"))
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
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
    
    public void AddUser(User user)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();

        // Hash the password before storing it
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        connection.Execute(@"
        INSERT INTO Users (FirstName, LastName, Email, Password, IsAdmin) 
        VALUES (@FirstName, @LastName, @Email, @Password, @IsAdmin)", user);
    }

    public bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }

    public IEnumerable<User> GetAllUsers()
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return connection.Query<User>("SELECT * FROM Users");
    }

    public static bool CheckIfUserExistByEmail(string email)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        return !(connection.Query<User>("SELECT * FROM Users WHERE Email = @email", new { email }).Count() == 0);
    }

    public static string GetUserEmail(string email)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        var result = connection.Query<User>("SELECT Email FROM Users WHERE Email = @email", new {email}).FirstOrDefault();
        
        if (result != null)
        {
            return result.Email;
        }
        else
        {
            return "";
        }
        
    }

    public static string GetUserPassword(string email)
    {
        using var connection = DbFactory.CreateConnection();
        connection.Open();
        var passwordHash = connection.Query<User>("SELECT Password FROM Users WHERE Email = @email", new {email}).FirstOrDefault();
        
        if (passwordHash != null)
        {
            return passwordHash.Password;
        }
        else
        {
            return "";
        }
        
    }
}