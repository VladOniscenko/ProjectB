namespace ProjectB.Presentation;
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Models;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;

public class UserProfile
{
    private readonly IServiceProvider _services;
    private readonly IReservationService _reservationService;

    public UserProfile(IServiceProvider services)
    {
        _services = services;
        _reservationService = _services.GetRequiredService<IReservationService>();
    }

    public void Run()
    {
        Console.Clear();
        switch (MenuSelection())
        {
            case "UM":
                ShowProfile();
                break;
            case "TK":
                ShowTickets();
                break;
            case "LO":
                Program.Logout();
                break;
            case "RM":
                return;
        }
    }

    public static string MenuSelection()
    {
        string title = "Select an option.\n";
        Dictionary<string, string> menuOptions = new()
        {
            { "UM", "Personal Information" },
            { "TK", "Reservations" },
            { "LO", "Log out" },
            { "RM", "Return to menu" },
        };

        var selectMenu = new Menu(title, menuOptions);
        return selectMenu.Run();
    }

    public void ShowProfile()
    {
        User? user = Program.CurrentUser;

        // Extra layer of protection just in case
        if (user == null)
        {
            Console.WriteLine("No user currently logged in. How did you manage to get here?");
            return;
        }
        
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║                Personal Information                ║");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.WriteLine($"║ First name:       {user.FirstName,-32} ║");
        Console.WriteLine($"║ Last name:        {user.FirstName,-32} ║");
        Console.WriteLine($"║ Email:            {user.Email,-32} ║");
        Console.WriteLine($"║ Password:         {user.Password,-32} ║");
        Console.WriteLine($"║ Is admin:         {(user.IsAdmin ? "Yes" : "No"),-32} ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.ReadLine();
        // IsAdmin is a 'ternary conditional'! Basically a shortened if/else lol. Note to remember that StackOverflow clutch knowledge.
    }

    public void ShowTickets()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  Reservations                      ║");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.ReadLine();
        
        User? user = Program.CurrentUser;

        // Extra layer of protection just in case
        if (user == null)
        {
            Console.WriteLine("║ No user logged in.                                ║");
            return;
        }
        else
        {
            var reservations = _reservationService.GetReservationsById(user.Id).ToList();

            if (reservations == null)
            {
                Console.WriteLine("║ No reservations found.                            ║");
            }
            else
            {
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"║ Reservation #{reservation.Id,-40}║");
                }
            }
        }

        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.ReadLine();
    }
}