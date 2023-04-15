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
            Console.WriteLine($"{i + 1}.\t{test.Name} finished {(test.Succeeded ? "Succesfully" : "Unsuccesfully")}");
            if (!test.Succeeded)
            {
                Console.WriteLine(test.Logs);
            }
        }
    }
}
