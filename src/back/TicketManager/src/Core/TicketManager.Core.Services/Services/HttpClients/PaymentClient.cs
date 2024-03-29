using System.Net;
using System.Net.Http.Json;
using TicketManager.Core.Contracts.Payments;

namespace TicketManager.Core.Services.Services.HttpClients;

public class PaymentClient
{
    private const string ApiKeyHeaderName = "pay_api_key";
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

    public async Task<Guid?> PostPaymentCreationAsync(CancellationToken ct)
    {
        try
        {
            var response = await client.PostAsync("payment/create", emptyContent, ct);
            var content = await response.Content.ReadFromJsonAsync<PaymentTokenDto>(cancellationToken: ct);
            return content!.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<PaymentStatusDto?> GetPaymentStatusAsync(CheckPaymentStatusRequest request, CancellationToken ct)
    {
        try
        {
            var response = await client.GetAsync($"payment/status?Id={request.Id}", ct);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var status = await response.Content.ReadFromJsonAsync<CheckPaymentStatusResponse>(cancellationToken: ct);
            return status?.Status;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
