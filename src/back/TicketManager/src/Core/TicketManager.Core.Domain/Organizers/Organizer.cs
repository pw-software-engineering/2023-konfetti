using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Organizer;

public class Organizer : IAggregateRoot<Guid>, IAccount
{
    public Guid Id { get; private init; }
    public string Email { get; private init; } = null!;
    public string CompanyName { get; private init; } = null!;
    public string Address { get; private init; } = null!;
    public string TaxId { get; private init; } = null!;
    public TaxIdType TaxIdType { get; private init; }
    public string DisplayName { get; private init; } = null!;
    public string PhoneNumber { get; private init; } = null!;

    public Organizer(string email, string companyName, string address, string taxId, TaxIdType taxIdType,
        string displayName, string phoneNumber)
    {
        Id = Guid.NewGuid();
        Email = email;
        CompanyName = companyName;
        Address = address;
        TaxId = taxId;
        TaxIdType = taxIdType;
        DisplayName = displayName;
        PhoneNumber = phoneNumber;
    }

    public Organizer() { }

    public Account GetAccount(string passwordHash)
    {
        return new Account(Id, Email, passwordHash, AccountRoles.Organizer);
    }
}

public enum TaxIdType
{
    Nip = 0,
    Regon = 1,
    Krs = 2,
    Pesel = 3,
    Vatin = 4
}
