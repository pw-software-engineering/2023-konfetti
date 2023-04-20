namespace CrossTeamTestSuite.TestsInfrastructure;

public abstract class MultiTest<TInput>
    where TInput : class
{
    public abstract string BaseName { get; }
    
    public abstract List<TInput> GetInputs();
    public abstract Task ExecuteAsync(TInput input);

    public List<Test> GetTests()
    {
        return GetInputs()
            .Select((input, index) => new Test(() => ExecuteAsync(input), $"{BaseName} {index + 1}"))
            .ToList();
    }
}
