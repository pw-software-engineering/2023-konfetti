using System.ComponentModel;
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

    public RoleTestInstance<T> AsAdmin()
    {
        roles.Should().Contain(AccountRoles.Admin);
        roles!.Remove(AccountRoles.Admin);
        return this;
    }
    
    public RoleTestInstance<T> AsOrganizer()
    {
        roles.Should().Contain(AccountRoles.Organizer);
        roles!.Remove(AccountRoles.Organizer);
        return this;
    }

    public RoleTestInstance<T> AsUser()
    {
        roles.Should().Contain(AccountRoles.User);
        roles!.Remove(AccountRoles.User);
        return this;
    }

    public RoleTestInstance<T> AsAnyRole()
    {
        roles.Should().Contain(AccountRoles.Admin);
        roles.Should().Contain(AccountRoles.Organizer);
        roles.Should().Contain(AccountRoles.User);
        roles!.Remove(AccountRoles.Admin);
        roles!.Remove(AccountRoles.Organizer);
        roles!.Remove(AccountRoles.User);
        return this;
    }

    public void CheckAnonymous()
    {
        roles.Should().BeNull();
    }

    public void Check()
    {
        roles.Should().BeEmpty();
    }
}
