using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.ApiKeyAuth;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.DtoMappers;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class ListPaymentsEndpoint: EndpointWithoutRequest<List<PaymentDto>>
{
    private readonly PaymentDbContext dbContext;
    private readonly PaymentServiceConfiguration configuration;

    public ListPaymentsEndpoint(PaymentDbContext dbContext, PaymentServiceConfiguration configuration)
    {
        this.dbContext = dbContext;
        this.configuration = configuration;
    }

    public override void Configure()
    {
        Get("/payment/list");
        PreProcessors(new ApiKeyAuthorizer<EmptyRequest>(configuration));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await dbContext
            .Payments
            .Select(PaymentDtoMapper.ToDtoMapper)
            .ToListAsync(ct);
        
        await SendOkAsync(result, ct);
    }
}
