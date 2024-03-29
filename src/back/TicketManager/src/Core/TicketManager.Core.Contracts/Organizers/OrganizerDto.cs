namespace TicketManager.Core.Contracts.Organizers;

public class OrganizerDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public TaxIdTypeDto TaxIdType { get; set; }
    public string TaxId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public VerificationStatusDto VerificationStatus { get; set; }
}

public enum VerificationStatusDto
{
    Unverified = 0,
    VerifiedPositively = 1,
    VerifiedNegatively = 2,
}
