using FastEndpoints;

namespace TicketManager.PaymentService.Services.Endpoints.HelloWorlds;

// TODO: remove it after creation another endpoint
public class HelloWorldEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/hello-world");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = "Hello world!";

        await SendOkAsync(result, ct);
    }
}
