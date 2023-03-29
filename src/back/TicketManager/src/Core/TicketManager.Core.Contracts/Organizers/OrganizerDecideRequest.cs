namespace TicketManager.Core.Contracts.Organizers;

public class OrganizerDecideRequest
{
    public Guid OrganizerId { get; set; }
    public bool IsAccepted { get; set; }
}
