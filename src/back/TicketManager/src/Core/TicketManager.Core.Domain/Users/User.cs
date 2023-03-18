using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Users;

public class User : IAggregateRoot<Guid>, IAccount
{
    public Guid Id { get; private init; }
    public string Email { get; private init; } = null!;
    public string FirstName { get; private init; } = null!;
    public string LastName { get; private init; } = null!;
    public DateOnly BirthDate { get; private init; }

    // DateTime IOptimisticConcurrent.DateModified { get; set; }
    
    private User() {}
    
    public User(string email, string firstName, string lastName, DateOnly birthDate)
    {
        Id = Guid.NewGuid();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
    
    public Account GetAccount(string passwordHash)
    {
        return new Account(Id, Email, passwordHash, AccountRoles.User);
    }
}
