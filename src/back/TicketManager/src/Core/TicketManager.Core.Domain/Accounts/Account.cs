using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Accounts;

public class Account : IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string Role { get; private init; } = null!;
    
    public DateTime DateModified { get; set; }
    
    private Account() {}
    
    public Account(Guid id, string email, string passwordHash, string role)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }
}
