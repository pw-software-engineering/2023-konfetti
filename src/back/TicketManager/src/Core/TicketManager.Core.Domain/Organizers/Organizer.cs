using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Organizer;

public class Organizer : IAggregateRoot<Guid>, IAccount, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public string Email { get; private set; } = null!;
    public string CompanyName { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxId { get; private set; } = null!;
    public TaxIdType TaxIdType { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public VerificationStatus VerificationStatus { get; private set; }
    public DateTime DateModified { get; set; }

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
        VerificationStatus = VerificationStatus.Unverified;
    }

    public Organizer() { }

    public Account GetAccount(string passwordHash)
    {
        return new Account(Id, Email, passwordHash, AccountRoles.Organizer);
    }

    public void Decide(bool isAccepted)
    {
        if(VerificationStatus != VerificationStatus.Unverified)
            return;
        VerificationStatus = isAccepted ? VerificationStatus.VerifiedPositively : VerificationStatus.VerifiedNegatively;
    }

    public bool IsVerified()
    {
        return VerificationStatus == VerificationStatus.VerifiedPositively;
    }

    public void Update(
        string email,
        string companyName,
        string address,
        string taxId,
        TaxIdType taxIdType,
        string displayName,
        string phoneNumber)
    {
        Email = email;
        CompanyName = companyName;
        Address = address;
        TaxId = taxId;
        TaxIdType = taxIdType;
        DisplayName = displayName;
        PhoneNumber = phoneNumber;
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

public enum VerificationStatus
{
    Unverified = 0,
    VerifiedPositively = 1,
    VerifiedNegatively = 2
}
