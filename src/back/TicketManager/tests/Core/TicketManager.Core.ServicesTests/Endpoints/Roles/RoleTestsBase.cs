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
    public CoreDbContext DbContext { get; set; }
    public Repository<Event, Guid> Events { get; set; }
    public Repository<Organizer, Guid> Organizers { get; set; }
    public Repository<User, Guid> Users { get; set; }
    public Repository<Account, Guid> Accounts { get; set; }
    public PasswordManager PasswordManager { get; set; }
    public TokenCreator TokenCreator { get; set; }

    private Dictionary<Type, object> singletons { get; set; }

    public RoleTestsBase()
    {
        singletons = new Dictionary<Type, object>();
        
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        DbContext = dbContextMock.Object;
        singletons.Add(typeof(CoreDbContext), DbContext);
        
        var eventsMock = new Mock<Repository<Event, Guid>>(DbContext);
        Events = eventsMock.Object;
        singletons.Add(typeof(Repository<Event, Guid>), Events);

        var organizersMock = new Mock<Repository<Organizer, Guid>>(DbContext);
        Organizers = organizersMock.Object;
        singletons.Add(typeof(Repository<Organizer, Guid>), Organizers);

        var usersMock = new Mock<Repository<User, Guid>>(DbContext);
        Users = usersMock.Object;
        singletons.Add(typeof(Repository<User, Guid>), Users);

        var accountMock = new Mock<Repository<Account, Guid>>(DbContext);
        Accounts = accountMock.Object;
        singletons.Add(typeof(Repository<Account, Guid>), Accounts);

        var passwordManagerMock = new Mock<PasswordManager>();
        PasswordManager = passwordManagerMock.Object;
        singletons.Add(typeof(PasswordManager), PasswordManager);
        
        var tokenCreatorMock = new Mock<TokenCreator>(new TokenConfiguration(""));
        TokenCreator = tokenCreatorMock.Object;
        singletons.Add(typeof(TokenCreator), TokenCreator);
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
