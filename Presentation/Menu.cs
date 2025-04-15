using static System.Console;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.DataAccess;
using ProjectB.Presentation;

// Used this video to help me out
// https://www.youtube.com/watch?v=qAWhGEPMlS8

namespace ProjectB
{
    class Menu
    {
        private int SelectedIndex = 0;
        private string[] Options;
        private string Prompt;

        public Menu(string prompt, string[] options)
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
            Clear();
            WriteLine(Prompt);
            
            WriteLine("╔══════════════════════════════════════╗");
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];

                if (i == SelectedIndex)
                {   
                    WriteLine($"║{CenterText(currentOption, 38, true)}║"); 
                }
                else
                {
                    WriteLine($"║{CenterText(currentOption, 38)}║"); 
                } 
            }
            WriteLine("╚══════════════════════════════════════╝");
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                // Code block to update SelectedIndex
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex = (SelectedIndex - 1 + Options.Length) % Options.Length;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex = (SelectedIndex + 1) % Options.Length;
                }
            }
            while (keyPressed != ConsoleKey.Enter);
 
            return SelectedIndex;
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
            string[] options = { "Register", "Login", "Movies", "About us", "Exit", "Create Movie (admin only)" };
            Menu menu = new Menu(prompt, options);
            int SelectedIndex = menu.Run();

            // Code block for keyPressed cases
            Console.Clear();
            switch(SelectedIndex)
            {
                case 0:
                    MenuActionRegister();
                    break;
                case 1:
                    Login();
                    break;
                case 2:
                    Movies();
                    break;
                case 3:
                    AboutUs();
                    break;
                case 4:
                    return;
                case 5:
                    MenuActionCreateMovie();
                    break;
            }

            RunMenu();
        }

        static void Login()
        {
            NotImplemented();
        }

        static void Movies()
        {    
            var movieRepo = new MovieRepository();
            
            MovieList movieList = new MovieList(movieRepo);
            movieList.OpenUserMenu();
        }

        static void AboutUs()
        {
            // NotImplemented();
            
            // todo this is temporary for the presentation
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
