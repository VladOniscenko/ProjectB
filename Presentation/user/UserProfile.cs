namespace ProjectB.Presentation;

public class UserProfile(IServiceProvider services)
{
    static string title = "Hi user! What would you like to do?\n";

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
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║                Personal Information                ║");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.ReadLine();
        // Rest of layout on the TPG 
    }

    public void ShowTickets()
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║                       Tickets                      ║");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.ReadLine();
        // Rest of layout on the TPG 
    }

    // Layout for personal information & tickets
// ╔════════════════════════════════════════════════════╗
// ║                    USER PROFILE                    ║
// ╠════════════════════════════════════════════════════╣
// ║ First name:       John                             ║
// ║ Last name:        Doe                              ║
// ║ Age:              20                               ║
// ║ Password:         *******                          ║
// ╠════════════════════════════════════════════════════╣
// ║                      CONTACT                       ║
// ║ Email:    john.doe@email.com                       ║
// ╚════════════════════════════════════════════════════╝
}