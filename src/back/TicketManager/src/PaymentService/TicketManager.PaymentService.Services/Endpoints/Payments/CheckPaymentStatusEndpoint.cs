using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.ApiKeyAuth;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CheckPaymentStatusEndpoint: Endpoint<CheckPaymentStatusRequest, CheckPaymentStatusResponse>
{
    private readonly PaymentDbContext dbContext;
    private readonly PaymentServiceConfiguration configuration;

    public CheckPaymentStatusEndpoint(PaymentDbContext dbContext, PaymentServiceConfiguration configuration)
    {
        this.dbContext = dbContext;
        this.configuration = configuration;
    }

    public override void Configure()
    {
        Get("/payment/status");
        PreProcessors(new ApiKeyAuthorizer<CheckPaymentStatusRequest>(configuration));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CheckPaymentStatusRequest req, CancellationToken ct)
    {
        var response = await dbContext
            .Payments
            .Where(p => p.Id == req.Id)
            .Select(p => new CheckPaymentStatusResponse
            {
                Status = p.HasExpired? PaymentStatusDto.Expired: (PaymentStatusDto)p.PaymentStatus
            })
            .FirstOrDefaultAsync(ct);

        if (response is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendOkAsync(response, ct);
    }
}
