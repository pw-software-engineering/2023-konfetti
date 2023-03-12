namespace TicketManager.Core.Domain.Accounts;

public class Account
{
    public Guid Id { get; private init; }
    public string Email { get; private init; }
    public string PasswordHash { get; private set; }
    
    public Account(Guid id, string email, string passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }
}
