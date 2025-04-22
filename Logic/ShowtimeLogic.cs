using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class ShowtimeLogic : IShowtimeService
{
    private readonly ShowtimeRepository _showtimeRepository;
    public ShowtimeLogic(ShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }
    
    public Showtime? Find(int id)
    {
        return _showtimeRepository.Find(id);
    }

    public IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 10)
    {
        return _showtimeRepository.GetShowtimesByMovieId(movieId, limit);
    }
    
       // No logic needed, but just in case.
    public bool IsMovieIDValid(string movie)
    {   
        if (string.IsNullOrWhiteSpace(movie))
        {
            BaseUI.ShowErrorMessage("\nNo input given. Please try again.\n", 0);
            return false;
        }

        Console.Clear();
        return true;
    }

    public bool CheckIfDataCorrect(string movie, int auditorium)
    {
        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine(
            $"    Movie:                {movie}\n    Auditorium:           {auditorium} \n    Begin time:           Placeholder\n    End time:             Placeholder");
        Console.WriteLine("\n   Is this correct?");
        Console.Write("    ");
        return BaseUI.BasicYesOrNo();
    }

    public void CreateShowtime(Showtime showtime)
    {
        _showtimeRepository.AddShowtime(showtime);
    }

     // create method to use keyboard arrows instead of console input 
    public int ShowMenuMovies(string title, List<Movie> options )
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

    public int ShowMenuAuditoriums(string title, List<Auditorium> options )
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
}