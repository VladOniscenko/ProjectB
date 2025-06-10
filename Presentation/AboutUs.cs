namespace ProjectB.Presentation;

public class AboutUs
{
    public AboutUs()
    {
        
    }

    public void Run()
    {
        Console.Clear();

        // Title
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                Welcome to Byte Cinemas                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("     More than just movies — it’s a cinematic experience.\n");
        Console.ResetColor();

        // Our Story
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("★ Our Story");
        Console.ResetColor();
        Console.WriteLine("  Founded by Jake Darcy, a film lover and entrepreneur,");
        Console.WriteLine("  Byte Cinemas exists to make every visit feel special.");
        Console.WriteLine("  From the moment you walk in to the final credits.\n");

        // Location
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("★ Location & Opening/Closing times");
        Console.ResetColor();
        Console.WriteLine("  Wijnhaven 107, 3011 WN — sleek, central, and easy to reach.\n");
        Console.WriteLine("  Monday — Saturday | 10:00 — 22:00");
        Console.WriteLine("  Sunday            | Closed\n");

        // Tech & Comfort
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("★ Tech & Comfort");
        Console.ResetColor();
        Console.WriteLine("  > Auro 3D sound and IMAX 3D digital projection");
        Console.WriteLine("  > Seating that suits you:");
        Console.WriteLine("     - Standard");
        Console.WriteLine("     - Premium");
        Console.WriteLine("     - VIP (plush seats, exclusive perks)\n");

        // Lounge & Bar
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("★ Lounge & Bar");
        Console.ResetColor();
        Console.WriteLine("  Modern lounge area to relax before or after your film.");
        Console.WriteLine("  Enjoy signature drinks and cinema-themed cocktails.\n");

        // Blah blah, yap yap!
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("★ For Everyone");
        Console.ResetColor();
        Console.WriteLine("  Whether you’re a casual viewer or a hardcore film fan,");
        Console.WriteLine("  Byte Cinemas gives you the movie experience you deserve.\n");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║            Movie nights done right—with Byte.          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        Console.ResetColor();

        BaseUI.ColoredText("Press Enter to return to menu", ConsoleColor.DarkGray);
        Console.ReadKey();
    }
}
