using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.DtoMappers;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class ListPaymentsEndpoint: EndpointWithoutRequest<ListPaymentsResponse>
{
    private readonly PaymentDbContext dbContext;

    public ListPaymentsEndpoint(PaymentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/payment/list");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var payments = await dbContext
            .Payments
            .Select(PaymentDtoMapper.ToDtoMapper)
            .ToListAsync(ct);

        var result = new ListPaymentsResponse()
        {
            Payments = payments
        };
        
        await SendOkAsync(result, ct);
    }
}