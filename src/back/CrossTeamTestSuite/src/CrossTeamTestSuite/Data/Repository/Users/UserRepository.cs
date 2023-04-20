namespace CrossTeamTestSuite.Data.Repository.Users;

public class UserRepository: IAccountRepository<User>
{
    private List<User> users = new();
    public string DefaultEmail { get; } = "user.ctts@email.com";
    public string DefaultPassword { get; } = "Password1";
    public User? DefaultAccount => users.FirstOrDefault(o => o.Email == DefaultEmail);
    public IReadOnlyList<User> Users => users.AsReadOnly();
    
    public void AddUser(User user)
    {
        users.Add(user);
    }
}
