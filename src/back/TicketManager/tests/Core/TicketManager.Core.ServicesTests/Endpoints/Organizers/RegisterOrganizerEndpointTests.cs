using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.PasswordManagers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Organizers;

public class RegisterOrganizerEndpointTests
{

    [Fact]
    public async Task WhenItIsCalled_ItShouldCallAddOnUsersAndAccounts()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var dbContext = dbContextMock.Object;
        var organizersMock = new Mock<Repository<Organizer, Guid>>(dbContext);
        var users = organizersMock.Object;
        var accountsMock = new Mock<Repository<Account, Guid>>(dbContext);
        var accounts = accountsMock.Object;
        var endpoint = Factory.Create<RegisterOrganizerEndpoint>(users, accounts, dbContextMock.Object, new PasswordManager());
        var req = new RegisterOrganizerRequest
        {
            CompanyName = "company sp. z o. o.",
            Address = "ul. Koszykowa 75, 00-662 Warszawa, Poland",
            TaxId = "3423272256",
            TaxIdType = TaxIdTypeDto.Nip,
            DisplayName = "Super company",
            Email = "email@emai.com",
            Password = "password",
            PhoneNumber = "123-456-789"
        };
        await endpoint.HandleAsync(req, default);

        organizersMock.Verify(u => u.Add(It.IsAny<Organizer>()), Times.Once);
        accountsMock.Verify(a => a.Add(It.IsAny<Account>()), Times.Once);
    }
}
