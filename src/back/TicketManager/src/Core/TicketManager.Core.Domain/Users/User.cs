using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Users;

public class User : IAggregateRoot<Guid>, IAccount, IOptimisticConcurrent
{
    private readonly List<UserFavouriteEvent> favouriteEvents = new();

    public Guid Id { get; private init; }
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }

    public IReadOnlyList<UserFavouriteEvent> FavouriteEvents => favouriteEvents.AsReadOnly();

    public DateTime DateModified { get; set; }
    
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

    public void Update(
        string email,
        string firstName,
        string lastName,
        DateOnly birthDate)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public void AddFavouriteEvents(UserFavouriteEvent favouriteEvent)
    {
        if (favouriteEvents.Contains(favouriteEvent))
        {
            throw new InvalidOperationException("Event can be add to favourites only once");
        }
        
        favouriteEvents.Add(favouriteEvent);
    }

    public void RemoveEventFromFavourites(Guid eventId)
    {
        favouriteEvents.RemoveAll(e => e.EventId == eventId);
    }
}

public record UserFavouriteEvent(Guid EventId);
