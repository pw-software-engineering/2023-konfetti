namespace TicketManager.PaymentService.Contracts.Payments;

public class ListPaymentsResponse
{
    public List<PaymentDto> Payments { get; set; }
}