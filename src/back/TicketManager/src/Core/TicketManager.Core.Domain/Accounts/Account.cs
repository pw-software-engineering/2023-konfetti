using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Accounts;

public class Account : IIdentifiable<Guid>
{
    public Guid Id { get; private init; }
    public string Email { get; private init; } = null!;
    public string PasswordHash { get; private set; } = null!;
    
    private Account() {}
    
    public Account(Guid id, string email, string passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }
}
