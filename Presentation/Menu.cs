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

        static public void RunMenu()
        {
            string prompt = @"
 ____             __               ____                                              __             
/\  _`\          /\ \__           /\  _`\    __                                     /\ \            
\ \ \L\ \  __  __\ \ ,_\    __    \ \ \/\_\ /\_\    ___      __    ___ ___      __  \ \/      ____  
 \ \  _ <'/\ \/\ \\ \ \/  /'__`\   \ \ \/_/_\/\ \ /' _ `\  /'__`\/' __` __`\  /'__`\ \/      /',__\ 
  \ \ \L\ \ \ \_\ \\ \ \_/\  __/    \ \ \L\ \\ \ \/\ \/\ \/\  __//\ \/\ \/\ \/\ \L\.\_      /\__, `\
   \ \____/\/`____ \\ \__\ \____\    \ \____/ \ \_\ \_\ \_\ \____\ \_\ \_\ \_\ \__/.\_\     \/\____/
    \/___/  `/___/> \\/__/\/____/     \/___/   \/_/\/_/\/_/\/____/\/_/\/_/\/_/\/__/\/_/      \/___/ 
               /\___/                                                                               
               \/__/                                                                                       

Welcome customer!
Use Up & Down keys to select an option.
                ";

            Dictionary<string, string> options = new()
            {
                { "UP", "Upcoming Movies" },
                { "AU", "About us" },
                { "LI", "Login" },
                { "RE", "Register" },
                { "EX", "Exit" },
                { "CM", "Create Movie (admins)" },
            };

            Menu menu = new Menu(prompt, options);

            string selectedOption = menu.Run();
            // Code block for keyPressed cases
            Console.Clear();
            switch (selectedOption)
            {
                case "RE":
                    MenuActionRegister();
                    break;
                case "LI":
                    Login();
                    break;
                case "UP":
                    MenuActionUpcomingMovies();
                    break;
                case "AU":
                    AboutUs();
                    break;
                case "EX":
                    return;
                case "CM":
                    MenuActionCreateMovie();
                    break;
            }

            RunMenu();
        }

        static void Login()
        {
            NotImplemented();
        }

        static void MenuActionUpcomingMovies()
        {
            MovieList movieList = new MovieList();
            movieList.Run();
        }

        static void AboutUs()
        {
            Console.WriteLine("=== Welcome to Byte Cinema ===");
            Console.WriteLine("Where storytelling meets cutting-edge technology.");
            Console.WriteLine("Experience ultra-crisp visuals, immersive sound, and an unforgettable atmosphere.");
            Console.WriteLine("From blockbusters to indie films – we bring stories to life, byte by byte.");
            Console.WriteLine("Sit back, relax, and enjoy the show.");
            
            Console.ReadKey();
        }        
        
        static void MenuActionCreateMovie()
        {
            CreateMovie.Create();
        }
        
        static void MenuActionRegister()
        {
            UserCreation.CreateUser();
        }


        static void NotImplemented()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not implemented yet");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
