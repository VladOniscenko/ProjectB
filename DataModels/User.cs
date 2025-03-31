namespace ProjectB.DataModels;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public List<Reservation> Reservations { get; set; } = new();

    public User(int id, string firstname, string lastname, string email, string password, List<Reservation> reservations, bool isAdmin)
    {
        Id = id;
        FirstName = firstname;
        LastName = lastname;
        Email = email;
        Password = password;
        Reservations = reservations;
        IsAdmin = isAdmin;
    }
    
    public static string Table()
    {
        return @"
            CREATE TABLE IF NOT EXISTS customer (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                IsAdmin INTEGER NOT NULL DEFAULT 0
            );
        ";
    }
}