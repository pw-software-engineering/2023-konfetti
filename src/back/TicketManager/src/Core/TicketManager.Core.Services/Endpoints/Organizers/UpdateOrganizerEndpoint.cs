using FastEndpoints;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class UpdateOrganizerEndpoint : Endpoint<UpdateOrganizerRequest>
{
    private readonly Repository<Organizer, Guid> organizers;

    public UpdateOrganizerEndpoint(Repository<Organizer, Guid> organizers)
    {
        this.organizers = organizers;
    }

    public override void Configure()
    {
        Post("/organizer/update");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(UpdateOrganizerRequest req, CancellationToken ct)
    {
        var organizer = await organizers.FindAndEnsureExistenceAsync(req.AccountId, ct);

        await organizers.UpdateAsync(organizer, ct);
    }
}
