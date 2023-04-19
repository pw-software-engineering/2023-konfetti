namespace CrossTeamTestSuite.Data.Repository.Users;

public class UserRepository: IAccountRepository
{
    private List<User> users = new();
    public string DefaultEmail { get; } = "user.ctts@email.com";
    public string DefaultPassword { get; } = "Password1";
    public IReadOnlyList<User> Users => users.AsReadOnly();
    
    public void AddUser(User user)
    {
        users.Add(user);
    }
}
