using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using Moq;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles;

public class RoleTestsBase
{
    public CoreDbContext DbContext { get; set; }
    public Repository<Event, Guid> Events { get; set; }
    public Repository<Organizer, Guid> Organizers { get; set; }
    public Repository<User, Guid> Users { get; set; }
    public Repository<Account, Guid> Accounts { get; set; }

    public RoleTestsBase()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        DbContext = dbContextMock.Object;
        var eventsMock = new Mock<Repository<Event, Guid>>(DbContext);
        Events = eventsMock.Object;
        var organizersMock = new Mock<Repository<Organizer, Guid>>(DbContext);
        Organizers = organizersMock.Object;
        var usersMock = new Mock<Repository<User, Guid>>(DbContext);
        Users = usersMock.Object;
        var accountMock = new Mock<Repository<Account, Guid>>(DbContext);
        Accounts = accountMock.Object;
    }
}
