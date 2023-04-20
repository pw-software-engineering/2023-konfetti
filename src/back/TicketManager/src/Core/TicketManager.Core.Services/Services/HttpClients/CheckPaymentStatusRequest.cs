namespace TicketManager.Core.Services.Services.HttpClients;

public class CheckPaymentStatusRequest
{
    public Guid Id { get; set; }
    
    public static class ErrorCodes
    {
        public static int PaymentDoesNotExist = 1;
    }
}
