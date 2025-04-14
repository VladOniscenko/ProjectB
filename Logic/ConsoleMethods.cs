namespace ProjectB.Logic;

public static class ConsoleMethods
{
    public static void AwaitUser()
    {
        Console.WriteLine(">> Press any key to continue");
        Console.ReadLine();
    }
    
    public static void Warning(string message) => WriteToConsole("Warning", message, ConsoleColor.Yellow);
    public static void Error(string message) => WriteToConsole("Error", message, ConsoleColor.Red);
    public static void Success(string message) => WriteToConsole("Success", message, ConsoleColor.Green);
    public static void WriteToConsole(string type, string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{type.ToUpper()}] {message}");
        Console.ResetColor();
        AwaitUser();
    }
}