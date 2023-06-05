using CrossTeamTestSuite.Endpoints.Contracts.Payments;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Payments;

public class PaymentConfirmInstance: EndpointInstance<PaymentConfirmRequest>
{
    public override async Task HandleEndpointAsync(PaymentConfirmRequest request)
    {
        await HttpClient.CallEndpointSuccessAsync(request);
    }
}
