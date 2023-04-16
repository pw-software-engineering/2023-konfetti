namespace CrossTeamTestSuite.Endpoints.Contracts.Common;

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = null!;
    public int TotalCount { get; set; }
}
