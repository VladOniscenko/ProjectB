using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Models;
using ProjectB.Presentation;

// Used this video to help me out
// https://www.youtube.com/watch?v=qAWhGEPMlS8

namespace ProjectB
{
    class Menu
    {
        private int SelectedIndex = 0;
        private Dictionary<string, string> Options;
        private string Prompt;

        public Menu(string prompt, Dictionary<string, string> options)
        {
            Prompt = prompt;
            Options = options;
        }

        public static string CenterText(string text, int boxWidth, bool isActive = false)
        {
            int spaces = boxWidth - text.Length;
            if (isActive)
            {
                spaces -= 6;
                text = $">> {text} <<";
            }
            int padleft = spaces / 2;
            int padright = spaces - padleft;
            return new string(' ', padleft) + new string($"{text}") + new string(' ', padright);
        }

        public void DisplayOptions()
        {
            Console.Clear();
            Console.WriteLine(Prompt);

            
            
            Console.WriteLine("╔══════════════════════════════════════╗");

            var optionLabels = Options.Values.ToList();
            for (int i = 0; i < optionLabels.Count; i++)
            {
                string currentOption = optionLabels[i];

                bool isSelected = i == SelectedIndex;
                Console.WriteLine($"║{CenterText(currentOption, 38, isSelected)}║");
            }
            Console.WriteLine("╚══════════════════════════════════════╝");

            List<Movie> Promotedmovie = new MovieLogic(new MovieRepository()).GetPromotedMovies();
            Console.WriteLine($"\nThis Week’s Top 3 Movies — Pick Your Favorite and Enjoy the Show!\n1: {Promotedmovie[0]}\n2: {Promotedmovie[1]}\n3: {Promotedmovie[2]}");

        }

        public string Run()
        {
            ConsoleKey keyPressed;
            List<string> optionKeys = Options.Keys.ToList();
            List<string> optionLabels = Options.Values.ToList();

            do
            {
                Console.Clear();
                Console.CursorVisible = false;
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                keyPressed = keyInfo.Key;

                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        SelectedIndex = (SelectedIndex - 1 + optionLabels.Count) % optionLabels.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        SelectedIndex = (SelectedIndex + 1) % optionLabels.Count;
                        break;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return optionKeys[SelectedIndex];
        }
        
        // create method to use keyboard arrows instead of console input 
        public static string SelectMenu(string title, Dictionary<string, string> options)
        {
            int selectedIndex = 0;
            ConsoleKey key;
            List<string> optionKeys = options.Keys.ToList();

            do
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine(new string('=', Console.WindowWidth));

                for (int i = 0; i < optionKeys.Count; i++)
                {
                    var value = options[optionKeys[i]];
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($"> {value} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {value}");
                    }
                    Console.WriteLine(new string('-', Console.WindowWidth));
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? optionKeys.Count - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % optionKeys.Count;
                        break;
                }

            } while (key != ConsoleKey.Enter);

            return optionKeys[selectedIndex];
        }
    }
}