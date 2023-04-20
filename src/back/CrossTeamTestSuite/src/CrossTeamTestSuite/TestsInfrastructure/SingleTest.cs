namespace CrossTeamTestSuite.TestsInfrastructure;

public abstract class SingleTest
{
    public abstract string Name { get; }
    
    public abstract Task ExecuteAsync();

    public Test GetTest()
    {
        return new Test(ExecuteAsync, Name);
    }
}
