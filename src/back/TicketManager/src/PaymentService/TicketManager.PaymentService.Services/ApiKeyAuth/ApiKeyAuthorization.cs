using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TicketManager.PaymentService.Services.Configuration;

namespace TicketManager.PaymentService.Services.ApiKeyAuth;

public class ApiKeyAuthorization<TRequest> : IPreProcessor<TRequest>
{
    private const string ApiKeyHeaderName = "pay_api_key";
    private PaymentServiceConfiguration configuration; 
    
    public ApiKeyAuthorization(PaymentServiceConfiguration configuration) {
        this.configuration = configuration;
    }
    public Task PreProcessAsync(TRequest req, HttpContext context, List<ValidationFailure> failures, CancellationToken ct)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out
                var extractedApiKey)) {
            context.Response.StatusCode = 401;
            return context.Response.SendForbiddenAsync(ct);
        }
        var apiKey = configuration.ApiKey;
        if (!apiKey.Equals(extractedApiKey)) {
            return context.Response.SendForbiddenAsync(ct);
        }

        return Task.CompletedTask;
    }
}
