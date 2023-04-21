namespace TicketManager.PaymentService.Contracts.Payments;

public class ConfirmPaymentRequest
{
    public Guid Id { get; set; }
    
    public static class ErrorCodes
    {
        public static int PaymentDoesNotExist = 1;
        public static int PaymentAlreadyDecided = 2;
        public static int PaymentHasExpired = 3;
    }
}
