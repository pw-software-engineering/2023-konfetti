using FastEndpoints;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class UpdateOrganizerEndpoint : Endpoint<UpdateOrganizerRequest>
{
    private readonly Repository<Organizer, Guid> organizers;
    private readonly Repository<Account, Guid> accounts;

    public UpdateOrganizerEndpoint(Repository<Organizer, Guid> organizers, Repository<Account, Guid> accounts)
    {
        this.organizers = organizers;
        this.accounts = accounts;
    }

    public override void Configure()
    {
        Post("/organizer/update");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(UpdateOrganizerRequest req, CancellationToken ct)
    {
        var organizer = await organizers.FindAndEnsureExistenceAsync(req.AccountId, ct);

        organizer.Update(
            req.Email ?? organizer.Email,
            req.CompanyName ?? organizer.CompanyName,
            req.Address ?? organizer.Address,
            req.TaxId ?? organizer.TaxId,
            (TaxIdType?)req.TaxIdType ?? organizer.TaxIdType,
            req.DisplayName ?? organizer.DisplayName,
            req.PhoneNumber ?? organizer.PhoneNumber);

        if (req.Email is not null)
        {
            var account = await accounts.FindAndEnsureExistenceAsync(req.AccountId, ct);
            
            account.UpdateEmail(req.Email);
            accounts.Update(account);
        }

        await organizers.UpdateAsync(organizer, ct);

        await SendOkAsync(ct);
    }
}
