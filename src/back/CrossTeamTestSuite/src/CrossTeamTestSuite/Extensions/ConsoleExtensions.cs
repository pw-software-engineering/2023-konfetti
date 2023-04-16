namespace CrossTeamTestSuite.Extensions;

public static class ConsoleExtensions
{
    public static void WriteLineWithColor(string text, ConsoleColor color)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = prevColor;
    }
    
    public static void WriteWithColor(string text, ConsoleColor color)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = prevColor;
    }
}
