using System.Net.Http.Json;
using TicketManager.Core.Contracts.Payments;

namespace TicketManager.Core.Services.Services.HttpClients;

public class PaymentClient
{
    private const string ApiKeyHeaderName = "ApiKey";
    private readonly HttpClient client;
    private readonly JsonContent emptyContent;

    public PaymentClient(HttpClient client)
    {
        this.client = client;
        emptyContent = JsonContent.Create("");
    }

    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService(typeof(PaymentClientConfiguration)) as PaymentClientConfiguration;
        client.BaseAddress = new Uri(configuration!.BaseUrl);
        client.DefaultRequestHeaders.Add(ApiKeyHeaderName, configuration.ApiKey);
    }

    public async Task<Guid?> PostPaymentCreation(CancellationToken ct)
    {
        var response = await client.PostAsync("payment/create", emptyContent, ct);
        var content = await response.Content.ReadFromJsonAsync<PaymentTokenDto>(cancellationToken: ct);
        return content!.Id;
    }
}
