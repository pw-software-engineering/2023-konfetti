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
        foreach (var test in tests)
        {
            await test.RunAsync();
            Console.WriteLine($"{test.Name} finished {(test.Succeeded ? "Succesfully" : "Unsuccesfully")}");
            if (!test.Succeeded)
            {
                Console.WriteLine(test.Logs);
            }
        }
    }
}
