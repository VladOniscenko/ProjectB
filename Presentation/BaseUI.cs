public static class BaseUI{
    const int BoxX = 15;
    const int Width = 30;

    public static string DrawInputBox(int x, int y, string label, string? previouslyWritten = null, bool isPassword = false)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(label + ": ");
        Console.SetCursorPosition(BoxX, y);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', Width));
        Console.SetCursorPosition(BoxX, y);
        return ReadInputBox(BoxX, y, Width, previouslyWritten, isPassword);
    }

    public static string ReadInputBox(int boxX, int y, int boxLength, string? previouslyWritten, bool isPassword)
    {
        string input = "";
        if (previouslyWritten is not null && !isPassword)
        {
            Console.SetCursorPosition(boxX, y);
            input = previouslyWritten;
            Console.Write(input);
            Console.SetCursorPosition(boxX + input.Length, y);
        }

        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.SetCursorPosition(boxX + input.Length, y);
                break;
            }

            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input.Remove(input.Length - 1);
                Console.SetCursorPosition(boxX + input.Length, y);
                Console.Write(" ");
                Console.SetCursorPosition(boxX + input.Length, y);
            }

            else if (!char.IsControl(key.KeyChar) && input.Length < boxLength - 1)
            {
                input += key.KeyChar;
                if (!isPassword)
                {
                    Console.Write(key.KeyChar);
                }
                else
                {
                    Console.Write("*");
                }
            }
        }

        Console.ResetColor();
        return input;
    }

    public static void ShowErrorMessage(int y, string errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, y);
        Console.Write("    " + errorMessage);
        Console.ResetColor();
    }

    public static bool BasicYesOrNo(){
        bool isAccepted = true;

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(" Yes ");
        Console.ResetColor();
        Console.Write("    No ");

        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.LeftArrow)
            {
                isAccepted = true;
                Console.SetCursorPosition(0, 7);
                Console.Write("    ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" Yes ");
                Console.ResetColor();
                Console.Write("    No   ");
            }

            if (key.Key == ConsoleKey.RightArrow)
            {
                isAccepted = false;
                Console.SetCursorPosition(0, 7);
                Console.Write("    ");
                Console.Write(" Yes    ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" No ");
                Console.ResetColor();
            }

            if (key.Key == ConsoleKey.Enter)
            {
                return isAccepted;
            }
    }
    }
}