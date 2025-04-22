namespace ProjectB.Presentation;

public static class ConsoleMethods
{
    public static void AwaitUser()
    {
        Console.WriteLine(">> Press any key to continue");
        Console.ReadLine();
    }
    
    public static void Warning(string message) => WriteToConsole(message, ConsoleColor.Yellow);
    public static void Error(string message) => WriteToConsole(message, ConsoleColor.Red);
    public static void Success(string message) => WriteToConsole(message, ConsoleColor.Green);
    public static void WriteToConsole(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"{message}");
        Console.ResetColor();
        AwaitUser();
    }
    
    public static void AnimateLoadingText(string text, int totalDurationMs = 2000)
    {
        int dotCount = 3;
        
        Console.Clear();
        int originalLeft = Console.CursorLeft;
        int originalTop = Console.CursorTop;
    
        DateTime endTime = DateTime.Now.AddMilliseconds(totalDurationMs);
        int currentDots = 0;
    
        while (DateTime.Now < endTime)
        {
            Console.SetCursorPosition(originalLeft, originalTop);
            Console.Write(text);
        
            string dots = new string('.', currentDots);
            dots = dots.PadRight(dotCount);
        
            Console.Write(dots);
            Thread.Sleep(200);
        
            currentDots = (currentDots + 1) % (dotCount + 1);
        }
    
        Console.SetCursorPosition(originalLeft, originalTop);
        Console.Write(new string(' ', text.Length + dotCount));
        Console.SetCursorPosition(originalLeft, originalTop);
        Console.WriteLine($"{text}...");
    }
}