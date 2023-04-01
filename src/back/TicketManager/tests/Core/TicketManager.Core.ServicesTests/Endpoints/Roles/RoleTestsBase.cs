using System.Reflection;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using Moq;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.Configuration;
using TicketManager.Core.Services.Services.PasswordManagers;
using TicketManager.Core.Services.Services.TokenManager;
using TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles;

public class RoleTestsBase
{
    private Dictionary<Type, object> singletons { get; set; }

    public RoleTestsBase()
    {
        singletons = new Dictionary<Type, object>();
        
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var dbContext = dbContextMock.Object;
        singletons.Add(typeof(CoreDbContext), dbContext);
        
        var eventsMock = new Mock<Repository<Event, Guid>>(dbContext);
        singletons.Add(typeof(Repository<Event, Guid>), eventsMock.Object);

        var organizersMock = new Mock<Repository<Organizer, Guid>>(dbContext);
        singletons.Add(typeof(Repository<Organizer, Guid>), organizersMock.Object);

        var usersMock = new Mock<Repository<User, Guid>>(dbContext);
        singletons.Add(typeof(Repository<User, Guid>), usersMock.Object);

        var accountMock = new Mock<Repository<Account, Guid>>(dbContext);
        singletons.Add(typeof(Repository<Account, Guid>), accountMock.Object);

        var passwordManagerMock = new Mock<PasswordManager>();
        singletons.Add(typeof(PasswordManager), passwordManagerMock.Object);
        
        var tokenCreatorMock = new Mock<TokenCreator>(new TokenConfiguration(""));
        singletons.Add(typeof(TokenCreator), tokenCreatorMock.Object);
    }

    public RoleTestInstance<T> GetRoleTestInstance<T>() where T : BaseEndpoint
    {
        var constructorInfo = typeof(T).GetConstructors()[0];
        var parameterInfo = constructorInfo.GetParameters();
        var parameters = new List<object>();

        foreach (var par in parameterInfo)
        {
            parameters.Add(singletons[par.ParameterType]);
        }

        return new RoleTestInstance<T>(parameters.ToArray());
    }
}
