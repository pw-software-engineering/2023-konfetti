namespace CrossTeamTestSuite.TestsInfrastructure;

public class TestPipeline
{
    private readonly List<Test> tests = new();

    public TestPipeline AddTest(SingleTest test)
    {
        tests.Add(test.GetTest());
        return this;
    }

    public TestPipeline AddMultiTest<TInput>(MultiTest<TInput> multiTest)
        where TInput : class
    {
        tests.AddRange(multiTest.GetTests());
        return this;
    }

    public TestExecutor GetExecutor()
    {
        return new TestExecutor(tests);
    }
}
