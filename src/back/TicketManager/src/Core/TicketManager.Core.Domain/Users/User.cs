using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Users;

public class User : IIdentifiable<Guid>, IAccount
{
    public Guid Id { get; private init; }
    public string Email { get; private init; } = null!;
    public string FirstName { get; private init; } = null!;
    public string LastName { get; private init; } = null!;
    public DateOnly BirthDate { get; private init; }
    
    private User() {}
    
    public User(Guid id, string email, string firstName, string lastName, DateOnly birthDate)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}
