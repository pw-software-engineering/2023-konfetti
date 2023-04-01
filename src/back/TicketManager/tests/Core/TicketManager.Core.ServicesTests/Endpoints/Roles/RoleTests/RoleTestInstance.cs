using FastEndpoints;
using FluentAssertions;
using TicketManager.Core.Domain.Accounts;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class RoleTestInstance<T> where T : BaseEndpoint
{
    private List<string>? roles;
    public RoleTestInstance(params object[] dependencies)
    {
        var endpoint = Factory.Create<T>(dependencies);
        endpoint.Configure();
        roles = endpoint.Definition.AllowedRoles;
    }

}
