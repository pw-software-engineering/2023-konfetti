namespace CrossTeamTestSuite;

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Wrong number of arguments provided");
        }

        var address = args[0];
        var adminEmail = args[1];
        var adminPassword = args[2];
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Tested app is at {address}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Admin email is {adminEmail}");
        Console.WriteLine($"Admin password is {adminPassword}");
    }
}

