using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Examples;

public record SumTestInput(int A, int B, int Expected);

public class SumTest : MultiTest<SumTestInput>
{
    public override string BaseName => "SumTest";
    public override List<SumTestInput> GetInputs()
    {
        return new()
        {
            new(0, 0, 0),
            new(1, 2, 3),
            new(-1, 1, 0),
            new(2, 2, 4),
        };
    }

    public override Task ExecuteAsync(SumTestInput input)
    {
        int actual = input.A + input.B;

        actual.Should().Be(input.Expected);
        
        return Task.CompletedTask;
    }
}
