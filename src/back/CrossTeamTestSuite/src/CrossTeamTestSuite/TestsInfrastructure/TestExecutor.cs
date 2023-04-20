using CrossTeamTestSuite.Extensions;

namespace CrossTeamTestSuite.TestsInfrastructure;

public class TestExecutor
{
    private readonly List<Test> tests = new();

    public TestExecutor(IEnumerable<Test> tests)
    {
        this.tests.AddRange(tests);
    }

    public async Task ExecuteAsync()
    {
        var failedCount = 0;
        foreach (var (test, i) in tests.WithIndex())
        {
            await test.RunAsync();
            WriteTestResult(test, i + 1);
            if (!test.Succeeded)
            {
                failedCount++;
                Console.WriteLine(test.Logs);
            }
        }
        
        Console.WriteLine();
        Console.WriteLine($"Total number of tests: {tests.Count}");
        ConsoleExtensions.WriteLineWithColor($"Passed tests: {tests.Count - failedCount}", ConsoleColor.Green);
        if (failedCount > 0)
        {
            ConsoleExtensions.WriteLineWithColor($"Failed tests: {failedCount}", ConsoleColor.Red);
        }
        else
        {
            ConsoleExtensions.WriteLineWithColor("Failed tests: 0", ConsoleColor.Green);
        }
    }

    private void WriteTestResult(Test test, int testNumber)
    {
        ConsoleExtensions.WriteWithColor($"{testNumber}.\t{test.Name} finished ",
            test.Succeeded ? ConsoleColor.Green : ConsoleColor.Red);
        if (test.Succeeded)
        {
            ConsoleExtensions.WriteLineWithColor("successfully", ConsoleColor.Green);
        }
        else
        {
            ConsoleExtensions.WriteLineWithColor("unsuccessfully", ConsoleColor.Red);
        }
    }
}
