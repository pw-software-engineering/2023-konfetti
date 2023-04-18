using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Account;

public class AccountLoginInstance: EndpointInstance<AccountLoginRequest, AccountLoginResponse>
{

    public override async Task<AccountLoginResponse?> HandleEndpointAsync(AccountLoginRequest request)
    {
        var token =  await HttpClient.CallEndpointAsync<AccountLoginRequest, AccountLoginResponse>(request);
        return token;
    }
}
