using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Examples;

public class MultiplyTest : SingleTest
{
    public override string Name => "MultiplyTest";
    
    public override Task ExecuteAsync()
    {
        int a = 4;
        int b = 5;
        int expected = 20;

        int actual = a * b;

        actual.Should().Be(expected);
        
        return Task.CompletedTask;
    }
}
