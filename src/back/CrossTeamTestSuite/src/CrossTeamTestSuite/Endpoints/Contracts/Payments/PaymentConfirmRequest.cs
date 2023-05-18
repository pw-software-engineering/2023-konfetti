using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Payments;

public class PaymentConfirmRequest: IRequest
{
    [JsonIgnore] 
    public string Path => "/payment/confirm";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid Id { get; set; }
}
