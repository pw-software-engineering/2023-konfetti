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
        foreach (var (test, i) in tests.WithIndex())
        {
            await test.RunAsync();
            WriteTestResult(test, i + 1);
            if (!test.Succeeded)
            {
                Console.WriteLine(test.Logs);
            }
        }
    }

    private void WriteTestResult(Test test, int testNumber)
    {
        Console.Write($"{testNumber}.\t{test.Name} finished ");
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
