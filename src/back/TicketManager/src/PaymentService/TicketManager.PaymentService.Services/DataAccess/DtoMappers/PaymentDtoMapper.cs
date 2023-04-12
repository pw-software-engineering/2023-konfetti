using System.Linq.Expressions;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;

namespace TicketManager.PaymentService.Services.DataAccess.DtoMappers;

public class PaymentDtoMapper
{
    public readonly static Expression<Func<Payment, PaymentDto>> ToDtoMapper = o => new PaymentDto()
    {
        Id = o.Id,
        PaymentStatus = (PaymentStatusDto)o.PaymentStatus,
        DateCreated = o.DateCreated
    };    
}
