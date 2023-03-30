namespace TicketManager.Core.Contracts.Organizers;

public class OrganizerDecideRequest
{
    public Guid OrganizerId { get; set; }
    public bool IsAccepted { get; set; }

    public static class ErrorCodes
    {
        public static int OrganizerNotInDatabase = 1;
        public static int OrganizerAlreadyVerified = 2;
    }
}
