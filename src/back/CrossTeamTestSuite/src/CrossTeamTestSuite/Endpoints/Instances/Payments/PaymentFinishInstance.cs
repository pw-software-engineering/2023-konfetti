using CrossTeamTestSuite.Endpoints.Contracts.Payments;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Payments;

public class PaymentFinishInstance: EndpointInstance<PaymentFinishRequest, PaymentFinishResponse>
{
    public override async Task<PaymentFinishResponse?> HandleEndpointAsync(PaymentFinishRequest request)
    {
        var response = await HttpClient.CallEndpointSuccessAsync<PaymentFinishRequest, PaymentFinishResponse>(request);
        
        return response;
    }
}

