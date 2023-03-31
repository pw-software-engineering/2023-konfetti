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
        var query = coreDbContext.Organizers.AsQueryable();

        if (req.CompanyNameFilter is not null)
            query = query.Where(o => o.CompanyName.StartsWith(req.CompanyNameFilter));
        if (req.AddressFilter is not null)
            query = query.Where(o => o.Address.StartsWith(req.AddressFilter));
        if (req.TaxIdFilter is not null)
            query = query.Where(o => o.TaxId.StartsWith(req.TaxIdFilter));
        if (req.TaxIdTypesFilter is not null)
            query = query.Where(o => req.TaxIdTypesFilter.Contains((TaxIdTypeDto)o.TaxIdType));
        if (req.DisplayNameFilter is not null)
            query = query.Where(o => o.DisplayName.StartsWith(req.DisplayNameFilter));
        if (req.EmailFilter is not null)
            query = query.Where(o => o.Email.StartsWith(req.EmailFilter));
        if (req.VerificationStatusesFilter is not null)
            query = query.Where(o =>
                req.VerificationStatusesFilter.Contains((VerificationStatusDto)o.VerificationStatus));

        Expression<Func<Organizer, string>> sortExpression = req.SortBy switch
        {
            OrganizerListSortByDto.Address => (o) => o.Address,
            OrganizerListSortByDto.Email => (o) => o.Email,
            OrganizerListSortByDto.CompanyName => (o) => o.CompanyName,
            OrganizerListSortByDto.DisplayName => (o) => o.DisplayName,
            _ => (o) => o.Id.ToString()
        };
        
        query = req.ShowAscending
            ? query.OrderBy(sortExpression)
            : query.OrderByDescending(sortExpression);
        
        var result = await query.Select(OrganizerDtoMapper.ToDtoMapper).ToPaginatedResponseAsync(req, ct);
        await SendAsync(result, cancellation: ct);
    }
}
