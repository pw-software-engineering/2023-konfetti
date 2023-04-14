namespace CrossTeamTestSuite.TestsInfrastructure;

public abstract class Test
{
    public bool Succeeded { get; private set; }
    public string Logs { get; private set; }
    
    public async Task RunAsync()
    {
        try
        {
            await ExecuteAsync();
            Succeeded = true;
        }
        catch (Exception e)
        {
            Logs = e.Message;
            Succeeded = false;
        }
    }

    public abstract Task ExecuteAsync();
}
