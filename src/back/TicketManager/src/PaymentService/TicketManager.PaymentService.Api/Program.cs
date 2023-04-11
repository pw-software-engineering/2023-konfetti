using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Validation;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<PaymentDbContext>(
            opts => opts
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseInMemoryDatabase("payment-db")
        );
        
        builder.Services.AddScoped<Repository<Payment, Guid>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc();
        
        var app = builder.Build();

        app.UseAuthorization();
        app.UseFastEndpoints(c =>
        {
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) => new ValidationErrorResponse
            {
                Errors = failures.Select(f => new ValidationError
                    {
                        ErrorCode = int.Parse(f.ErrorCode),
                        ErrorMessage = f.ErrorMessage,
                    })
                    .ToList(),
            };
        });
        app.UseSwaggerGen();

        app.Run();
    }
}
