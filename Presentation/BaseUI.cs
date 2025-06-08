using Microsoft.VisualStudio.TestPlatform.Utilities;

public static class BaseUI
{
    private static int BoxX { get; set; }
    private static int Width { get; set; }
    //make width boXx AND X Y OPTIONAL

    static BaseUI()
    {
        BoxX = 15;
        Width = 30;
    }

    public static string DrawInputBox(string label, int BoxX = 15, int Width = 30, int x = -1, int y = -1,
        string? previouslyWritten = null, bool isPassword = false)
    {
        if (x == -1)
        {
            x = Console.CursorLeft;
        }

        if (y == -1)
        {
            y = Console.CursorTop;
        }

        Console.SetCursorPosition(x, y);
        Console.Write(label + ": ");
        Console.SetCursorPosition(BoxX, y);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', Width));
        Console.SetCursorPosition(BoxX, y);
        return ReadInputBox(BoxX, Width, isPassword, y, previouslyWritten);
    }

    public static string ReadInputBox(int boxX, int boxLength, bool isPassword, int y, string? previouslyWritten)
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

            if (key.Key == ConsoleKey.Escape)
            {
                return null;
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

    public static void ShowErrorMessage(string errorMessage, int? yAxis = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (yAxis is int Y)
        {
            Console.SetCursorPosition(0, Y);
        }

        Console.Write("" + errorMessage);
        Console.ResetColor();
    }

    public static void ConfirmingMessage(string confirmingMessage, int? yAxis = null)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        if (yAxis is int Y)
        {
            Console.SetCursorPosition(0, Y);
        }

        Console.Write("    " + confirmingMessage);
        Console.ResetColor();
    }

    public static bool BasicYesOrNo(int y)
    {
        bool isAccepted = true;

        Console.SetCursorPosition(0, y);
        Console.Write("    ");
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
                Console.SetCursorPosition(0, y);
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
                Console.SetCursorPosition(0, y);
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

    public static bool BasicYesOrNo()
    {
        int y = Console.CursorTop;
        return BasicYesOrNo(y + 1);
    }

    public static void ResetColor()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }
}