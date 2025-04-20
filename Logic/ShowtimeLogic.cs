using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public static class ShowtimeLogic
{
    // No logic needed, but just in case.
    public static bool IsMovieIDValid(string movie)
    {   
        if (string.IsNullOrWhiteSpace(movie))
        {
            BaseUI.ShowErrorMessage("\nNo input given. Please try again.\n", 0);
            return false;
        }

    Console.Clear();
    return true;
    }

    public static bool CheckIfDataCorrect(string movie, int auditorium)
    {
        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine(
            $"    Movie:                {movie}\n    Auditorium:           {auditorium} \n    Begin time:           Placeholder\n    End time:             Placeholder");
        Console.WriteLine("\n   Is this correct?");
        Console.Write("    ");
        return BaseUI.BasicYesOrNo();
    }

    public static void CreateShowtime(Showtime showtime)
    {
        ShowtimeRepository showtimeRepository = new ShowtimeRepository();
        showtimeRepository.AddShowtime(showtime);
    }

     // create method to use keyboard arrows instead of console input 
    public static int ShowMenuMovies(string title, List<Movie> options )
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();
 
        do
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine(title);
            Console.WriteLine(new string('═', title.Length));

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($">> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {options[i]}");
                }
                // Spaces between titles looks ugly tbh.
                // Console.WriteLine(new string('-', options[i].Length));
            }

            if (options.Count == 0)
            {   Console.WriteLine();
                BaseUI.ShowErrorMessage("\nNo movies found with this title/keyword.", 6);
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected == 0) ? options.Count - 1 : selected - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected == options.Count - 1) ? 0 : selected + 1;
                    break;
            }

        } while (key != ConsoleKey.Enter);

        return selected;
    }

    public static int ShowMenuAuditoriums(string title, List<Auditorium> options )
    {
        int selected = 0;
        ConsoleKey key;
        List<string> writtenLines = new();
 
        do
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine(title);
            Console.WriteLine(new string('═', title.Length));

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($">> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {options[i]}");
                }
                // Spaces between titles looks ugly tbh.
                // Console.WriteLine(new string('-', options[i].Length));
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected == 0) ? options.Count - 1 : selected - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected == options.Count - 1) ? 0 : selected + 1;
                    break;
            }

        } while (key != ConsoleKey.Enter);

        return selected;
    }

    /// <summary>
    /// Adds a menu to the console from a specified start row.
    /// </summary>
    /// <param name="title"> Title of the menu </param>
    /// <param name="options"> Options the menu provides </param>
    /// <param name="startRow"> Row from where to start the menu </param>
    /// <param name="rowBuffer"> Used in addition to the startRow param to maniupulate the actual start of writing the menu. </param>
    /// <returns></returns>
}

