using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class OrganizerViewEndpoint : Endpoint<OrganizerViewRequest>
{
    private readonly CoreDbContext dbContext;

    public OrganizerViewEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/organizer/view");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(OrganizerViewRequest req, CancellationToken ct)
    {
        var response = await dbContext
            .Organizers
            .Select(o => new OrganizerDto
            {
                Id = o.Id,
                Address = o.Address,
                CompanyName = o.CompanyName,
                DisplayName = o.DisplayName,
                Email = o.Email,
                PhoneNumber = o.PhoneNumber,
                TaxIdType = (TaxIdTypeDto)o.TaxIdType,
                VerificationStatus = VerificationStatusDto.VerifiedPositively, // TODO: fix it
            })
            .FirstOrDefaultAsync(o => o.Id == req.AccountId, ct);

        if (response is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendAsync(response, cancellation: ct);
    }
}
