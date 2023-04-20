namespace CrossTeamTestSuite.TestsInfrastructure;

public class Test
{
    private readonly Func<Task> testExecutor;

    public bool Succeeded { get; private set; }
    public string Logs { get; private set; } = "";
    public string Name { get; private init; }

    public Test(Func<Task> testExecutor, string name = "Test")
    {
        this.testExecutor = testExecutor;
        Name = name;
    }
    
    public async Task RunAsync()
    {
        try
        {
            await testExecutor();
            Succeeded = true;
        }
        catch (Exception e)
        {
            Logs = e.Message;
            Succeeded = false;
        }
    }
}
