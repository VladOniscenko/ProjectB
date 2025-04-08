using static System.Console;

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
            string[] options = { "Register", "Login", "Movies", "About us", "Exit" };
            Menu menu = new Menu(prompt, options);
            int SelectedIndex = menu.Run();

            // Code block for keyPressed cases
            switch(SelectedIndex)
            {
                case 0:
                    Console.Clear();
                    Register();
                    break;
                case 1:
                    Console.Clear();
                    Login();
                    break;
                case 2:
                    Console.Clear();
                    Movies();
                    break;
                case 3:
                    Console.Clear();
                    AboutUs();
                    break;
                case 4:
                    Console.Clear();
                    return;
            }
        }

        // Methods for each option in menu. Might change/remove?
        static void Register()
        {
            WriteLine("Works.");
        }

        static void Login()
        {
            WriteLine("Works.");
        }

        static void Movies()
        {
            WriteLine("Works.");
        }

        static void AboutUs()
        {
            WriteLine("Works.");
        }
    }
}
