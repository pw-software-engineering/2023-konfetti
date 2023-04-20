using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;

namespace TicketManager.PaymentService.ServicesTests.Helpers;

public static class ToDtoConverters
{
    public static PaymentDto PaymentToDto(Payment payment)
    {
        return new PaymentDto()
        {
            Id = payment.Id,
            PaymentStatus = payment.HasExpired? PaymentStatusDto.Expired: (PaymentStatusDto)payment.PaymentStatus,
            DateCreated = payment.DateCreated
        };
    }
}
