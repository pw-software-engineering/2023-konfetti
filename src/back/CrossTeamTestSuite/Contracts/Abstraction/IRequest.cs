namespace CrossTeamTestSuite.Contracts.Abstraction;

public interface IRequest<TResponse>
    where TResponse : class
{
    public string Path { get; }
    public RequestType Type { get; }
}

public interface IRequest
{
    public string Path { get; }
    public RequestType Type { get; }
}
