using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Users;

public class User : IIdentifiable<Guid>, IAccount
{
    public Guid Id { get; private init; }
    public string Email { get; private init; }
    public string FirstName { get; private init; }
    public string LastName { get; private init; }
    public DateOnly BirthDate { get; private init; }
    
    public User(Guid id, string email, string firstName, string lastName, DateOnly birthDate)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}
