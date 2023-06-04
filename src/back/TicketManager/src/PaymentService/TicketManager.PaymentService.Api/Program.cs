using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Validation;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.Repositories;
using TicketManager.PaymentService.Services.Services.Mockables;

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
        
        builder.Services.AddSingleton<MockablePaymentDbResolver>();
        builder.Services.AddSingleton(new PaymentServiceConfiguration(builder.Configuration["PaymentClientApiKey"]));
        
        builder.Services.AddScoped<Repository<Payment, Guid>>();
        
        builder.Services.AddFastEndpoints(options =>
        {
            options.AssemblyFilter = assembly => assembly.FullName?.Contains("TicketManager.PaymentService.Services") ?? false;
        });
        
        builder.Services.AddSwaggerDoc();
        
        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(host => host.StartsWith("http://localhost"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));
        
        var app = builder.Build();

        app.UsePathBase("/pay/");
        app.UseRouting();
        app.UseLoggingMiddleware();

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
