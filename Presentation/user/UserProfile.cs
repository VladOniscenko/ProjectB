namespace ProjectB.Presentation;
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Models;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;

public class UserProfile
{
    const int BoxX = 20;
    const int Width = 35;
    private readonly IServiceProvider _services;
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;

    public UserProfile(IServiceProvider services)
    {
        _services = services;
        _reservationService = _services.GetRequiredService<IReservationService>();
        _userService = _services.GetRequiredService<IUserService>();
    }

    // Cases for profile menu
    public void Run()
    {
        while (true)
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
                    return;
                case "RM":
                    return;
            }
        }
    }

    // Cases for profile editing menu
    public void RunEdit()
    {
        while (true)
        {
            User? user = Program.CurrentUser;
            Console.Clear();
            switch (EditMenuSelection())
        {
            case "FN":
                EditFirstName(user);
                break;
            case "LN":
                EditLastName(user);
                break;
            case "EM":
                EditEmail(user);
                break;
            case "PW":
                EditPassword(user);
                break;
            case "EX":
                return;
            }
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

    public static string EditMenuSelection()
    {
        string title = "Select information to edit.\n";
        Dictionary<string, string> menuOptions = new()
        {
            { "FN", "First name" },
            { "LN", "Last name" },
            { "EM", "Email" },
            { "PW", "Password" },
            { "EX", "Exit" },
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
        Console.WriteLine($"║ Last name:        {user.LastName,-32} ║");
        Console.WriteLine($"║ Email:            {user.Email,-32} ║");
        Console.WriteLine($"║ Password:         {"*********",-32} ║");
        Console.WriteLine($"║ Is admin:         {(user.IsAdmin ? "Yes" : "No"),-32} ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝\n");
        Console.WriteLine("Press 'Backspace' to edit personal information.");
        // IsAdmin is a 'ternary conditional'! Basically a shortened if/else lol. Note to remember that StackOverflow clutch knowledge.

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Backspace)
        {
            RunEdit();
        }
    }

    public void ShowTickets()
    {
        Console.Clear();
        var user = Program.CurrentUser;

        if (user == null)
        {
            Console.WriteLine("No user logged in.");
            Console.ReadLine();
            return;
        }

        var reservations = _reservationService.GetReservationsById(user.Id).ToList();

        if (!reservations.Any())
        {
            Console.WriteLine("No reservations found.");
        }
        else
        {
        foreach (var reservation in reservations)
        {
            Console.WriteLine(_reservationService.GetReservationInfo(reservation));
            Console.WriteLine();
        }
    }

    Console.ReadLine();

    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// Using Dmitiri's output checks for this!
    private void EditFirstName(User user)
    {
        Console.Clear();
        user.FirstName = BaseUI.DrawInputBox("First name", BoxX, Width, 0, 4, user.FirstName);
        while (!_userService.IsNameValid(user.FirstName))
        {
            BaseUI.ShowErrorMessage("Name must be longer than 3 characters and can only contain letters", 5);
            user.FirstName = BaseUI.DrawInputBox("First name", BoxX, Width, 0, 4, user.FirstName);
        }
        _userService.UpdateUser(user);
    }

    private void EditLastName(User user)
    {
        Console.Clear();
        user.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 4, user.LastName);
        while (!_userService.IsNameValid(user.LastName))
        {
            BaseUI.ShowErrorMessage("Name must be longer than 3 characters and can only contain letters", 5);
            user.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 4, user.LastName);
        }
        _userService.UpdateUser(user);
    }

    private void EditEmail(User user)
    {
        Console.Clear();
        user.Email = BaseUI.DrawInputBox("Email", BoxX, Width, 0, 8, user.Email);
        while (!_userService.IsEmailValid(user.Email) || _userService.DoesUserExist(user.Email))
        {
            if (!_userService.IsEmailValid(user.Email))
                BaseUI.ShowErrorMessage("Please enter a valid email address", 9);
            else
                BaseUI.ShowErrorMessage("Account with this email already exists", 9);

            user.Email = BaseUI.DrawInputBox("Email", BoxX, Width, 0, 8, user.Email);
        }
        _userService.UpdateUser(user);
    }

    private void EditPassword(User user)
    {
        Console.Clear();
        string confirmPassword = "-";

        while (!_userService.IsPasswordIdentical(user.Password, confirmPassword))
        {
            user.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, user.Password, true);

            while (!_userService.IsPasswordValid(user.Password))
            {
                BaseUI.ShowErrorMessage("Please enter a valid password (at least 8 characters)", 11);
                user.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, user.Password, true);
            }

            Console.SetCursorPosition(0, 11);
            Console.Write("                                                                                                   ");

            confirmPassword = BaseUI.DrawInputBox("Re-enter password", BoxX, Width, 0, 12, user.Password, true);

            if (!_userService.IsPasswordIdentical(user.Password, confirmPassword))
            {
                BaseUI.ShowErrorMessage("Passwords do not match", 13);
                Console.SetCursorPosition(BoxX, 12);
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(new string(' ', Width));
                Console.ResetColor();
            }
        }

        _userService.UpdateUser(user);

    }
}