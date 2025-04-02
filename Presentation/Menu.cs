using System;
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
        
        public void DisplayOptions()
        {
            Console.Clear();
            WriteLine(Prompt);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];

                if (i == SelectedIndex)
                {
                    Console.WriteLine($">> {currentOption} <<"); 
                }
                else
                {
                    Console.WriteLine($"   {currentOption}");
                }
            }
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
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

            // Code block for keyPressed cases
 
            return SelectedIndex;
        }


        public void Start()
        {
            DisplayOptions();
        }

    }
}
