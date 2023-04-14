namespace CrossTeamTestSuite.TestsInfrastructure;

public abstract class SingleTest
{
    public abstract Task ExecuteAsync();

    public Test GetTest()
    {
        return new Test(ExecuteAsync);
    }
}
