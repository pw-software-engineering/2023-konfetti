namespace TicketManager.PaymentService.Contracts.Payments;

public class PaymentDto
{
    public Guid Token { get; set;  }
    public PaymentStatusDto PaymentStatus { get; set;  }
    public DateTime DateCreated { get; set; }
}
