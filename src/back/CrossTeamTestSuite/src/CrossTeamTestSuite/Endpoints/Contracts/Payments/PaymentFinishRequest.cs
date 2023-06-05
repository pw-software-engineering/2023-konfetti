using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Payments;

public class PaymentFinishRequest: IRequest<PaymentFinishResponse>
{
    [JsonIgnore]
    public string Path => "/payment/finish";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid PaymentId { get; set; }
}
