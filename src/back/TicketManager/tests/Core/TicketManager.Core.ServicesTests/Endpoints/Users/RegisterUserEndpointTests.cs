using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.PasswordManagers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Users;

public class RegisterUserEndpointTests
{
   [Fact]
    public async Task WhenItIsCalled_ItShouldCallAddOnUsersAndAccounts()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var dbContext = dbContextMock.Object;
        var usersMock = new Mock<Repository<User, Guid>>(dbContext);
        var users = usersMock.Object;
        var accountsMock = new Mock<Repository<Account, Guid>>(dbContext);
        var accounts = accountsMock.Object;
        var endpoint = Factory.Create<RegisterUserEndpoint>(users, accounts, dbContextMock.Object, new PasswordManager());

        var req = new RegisterUserRequest
        {
            Email = "email@emai.com",
            FirstName = "name",
            LastName = "lastName",
            Password = "password",
            BirthDate = new DateOnly(2020, 1, 1),
        };
        await endpoint.HandleAsync(req, default);

        usersMock.Verify(u => u.Add(It.IsAny<User>()), Times.Once);
        accountsMock.Verify(a => a.Add(It.IsAny<Account>()), Times.Once);
    }
}
