namespace CrossTeamTestSuite.TestsInfrastructure;

public abstract class MultiTest<TInput>
    where TInput : class
{
    public abstract List<TInput> GetInputs();
    public abstract Task ExecuteAsync(TInput input);

    public List<Test> GetTests()
    {
        return GetInputs()
            .Select(i => new Test(() => ExecuteAsync(i)))
            .ToList();
    }
}
