using System.Linq.Expressions;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;

namespace TicketManager.PaymentService.Services.DataAccess.DtoMappers;

public class PaymentDtoMapper
{
    public readonly static Expression<Func<Payment, PaymentDto>> ToDtoMapper = p => new PaymentDto()
    {
        Id = p.Id,
        PaymentStatus = p.HasExpired? PaymentStatusDto.Expired: (PaymentStatusDto)p.PaymentStatus,
        DateCreated = p.DateCreated
    };    
}
