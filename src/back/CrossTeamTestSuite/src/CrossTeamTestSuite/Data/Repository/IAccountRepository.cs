namespace CrossTeamTestSuite.Data.Repository;

public interface IAccountRepository<TAccount>: IRepository
    where TAccount: class
{
    public string DefaultEmail { get; }
    public string DefaultPassword { get; }
    public TAccount? DefaultAccount { get; }
}
