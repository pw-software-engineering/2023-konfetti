namespace TicketManager.Core.Services.Services.HttpClients;

public class PaymentClient
{
    private const string ApiKeyHeaderName = "ApiKey";
    private readonly HttpClient client;

    public PaymentClient(HttpClient client)
    {
        this.client = client;
    }

    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService(typeof(PaymentClientConfiguration)) as PaymentClientConfiguration;
        client.BaseAddress = new Uri(configuration!.BaseUrl);
        client.DefaultRequestHeaders.Add(ApiKeyHeaderName, configuration.ApiKey);
    }
}
