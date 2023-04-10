namespace TicketManager.Core.Contracts.Tickets;

public class TicketBuyRequest
{
    public Guid EventId { get; set; }
    public string SectorName { get; set; }
    public int NumberOfSeats { get; set; }
    
    public static class ErrorCodes
    {
        public static int EventDoesNotExist = 1;
        public static int SectorNameDoesNotExist = 2;
    }
}
