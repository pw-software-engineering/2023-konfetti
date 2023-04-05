using FastEndpoints;
using FastEndpoints.Swagger;
using TicketManager.PaymentService.Contracts.Validation;

namespace TicketManager.PaymentService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
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
