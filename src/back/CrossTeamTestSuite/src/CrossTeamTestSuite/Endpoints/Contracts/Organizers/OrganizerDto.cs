namespace CrossTeamTestSuite.Endpoints.Contracts.Organizers;

public class OrganizerDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public TaxIdTypeDto TaxIdType { get; set; }
    public string TaxId { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public VerificationStatusDto VerificationStatus { get; set; }
}
