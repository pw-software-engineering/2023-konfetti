using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Services.Mockables;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Organizers;

public class OrganizerDecideValidatorTests
{
    private Organizer organizer;
    private OrganizerDecideValidator validator;
    private OrganizerDecideRequest request;
    
    public OrganizerDecideValidatorTests()
    {
        organizer = new Organizer("email@email.com", "companyName", "address",
            "taxId", TaxIdType.Nip, "dispalyName", "phoneNumber");
        validator = GetValidator(new List<Organizer>() { organizer });

        request = new OrganizerDecideRequest
        {
            OrganizerId = organizer.Id,
            IsAccepted = true
        };
    }

    [Fact]

    public async Task WhenValidRequestIsProvided_ItShouldReturnTrue()
    {
        var result = await validator.ValidateAsync(request);
        result.EnsureCorrectness();
    }

    [Fact]
    public async Task WhenIdIsNotInDatabase_ItShouldReturnFalseWithOrganizerNotInDatabaseErrorCode()
    {
        request.OrganizerId = Guid.Empty;
        var result = await validator.ValidateAsync(request);
        result.EnsureCorrectError(OrganizerDecideRequest.ErrorCodes.OrganizerNotInDatabase);
    }

    [Fact]
    public async Task WhenOrganizerAlreadyVerified_ItShouldReturnFalseWithOrganizerAlreadyVerifiedErrorCode()
    {
        organizer.Decide(true);
        var result = await validator.ValidateAsync(request);
        result.EnsureCorrectError(OrganizerDecideRequest.ErrorCodes.OrganizerAlreadyVerified);
    }
    private static OrganizerDecideValidator GetValidator(List<Organizer> organizers)
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Organizers).ReturnsDbSet(organizers);
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new OrganizerDecideValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        return validator;
    }
}
