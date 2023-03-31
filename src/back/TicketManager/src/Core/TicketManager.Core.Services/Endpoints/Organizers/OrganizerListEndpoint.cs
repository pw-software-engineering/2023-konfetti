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

        Func<Organizer, string> sortFunc = req.SortBy switch
        {
            OrganizerListSortByDto.Address => (Organizer o) => o.Address,
            OrganizerListSortByDto.Email => (Organizer o) => o.Email,
            OrganizerListSortByDto.CompanyName => (Organizer o) => o.CompanyName,
            OrganizerListSortByDto.DisplayName => (Organizer o) => o.DisplayName,
            _ => (Organizer o) => o.Id.ToString()
        };

        query = req.ShowAscending
            ? query.AsEnumerable().OrderBy(sortFunc).AsQueryable()
            : query.AsEnumerable().OrderByDescending(sortFunc).AsQueryable();
        
        var result = await query.Select(OrganizerDtoMapper.ToDtoMapper).ToPaginatedResponseAsync(req, ct);
        await SendAsync(result, cancellation: ct);
    }
}
