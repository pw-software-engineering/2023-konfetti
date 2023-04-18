namespace CrossTeamTestSuite.Data.Repository;

public interface IAccountRepository: IRepository
{
    public string DefaultEmail { get; }
    public string DefaultPassword { get; }
}
