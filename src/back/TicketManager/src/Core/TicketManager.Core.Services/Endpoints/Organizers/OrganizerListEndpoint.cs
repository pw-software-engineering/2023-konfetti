using System.Linq.Expressions;
using FastEndpoints;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.DtoMappers;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class OrganizerListEndpoint: Endpoint<OrganizerListRequest, PaginatedResponse<OrganizerDto>>
{
    private readonly CoreDbContext coreDbContext;

    public OrganizerListEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public override void Configure()
    {
        Get("/organizer/list");
        Roles(AccountRoles.Admin);
    }

    public override async Task HandleAsync(OrganizerListRequest req, CancellationToken ct)
    {
        var query = coreDbContext.Organizers.AsQueryable()
            .FilterStringField(o => o.CompanyName, req.CompanyNameFilter)
            .FilterStringField(o => o.Address, req.AddressFilter)
            .FilterStringField(o => o.TaxId, req.TaxIdFilter)
            .FilterStringField(o => o.DisplayName, req.DisplayNameFilter)
            .FilterStringField(o => o.Email, req.EmailFilter)
            .FilterListField(o => o.TaxIdType, req.TaxIdTypesFilter)
            .FilterListField(o => o.VerificationStatus, req.VerificationStatusesFilter);

        Expression<Func<Organizer, string>> sortExpression = req.SortBy switch
        {
            OrganizerListSortByDto.Address => (o) => o.Address,
            OrganizerListSortByDto.Email => (o) => o.Email,
            OrganizerListSortByDto.CompanyName => (o) => o.CompanyName,
            OrganizerListSortByDto.DisplayName => (o) => o.DisplayName,
            _ => (o) => o.Id.ToString()
        };
        
        var result = await query
            .SortBy(sortExpression, req)
            .Select(OrganizerDtoMapper.ToDtoMapper)
            .ToPaginatedResponseAsync(req, ct);
        await SendAsync(result, cancellation: ct);
    }
}
