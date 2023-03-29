using FastEndpoints;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class OrganizerDecideEndpoint: Endpoint<OrganizerDecideRequest>
{
    private Repository<Organizer, Guid> organizerRepository;
    
    public OrganizerDecideEndpoint(Repository<Organizer, Guid> organizerRepository)
    {
        this.organizerRepository = organizerRepository;
    }

    public override void Configure()
    {
        Post("/organizer/decide");
        Roles(AccountRoles.Admin);
    }

    public override async Task HandleAsync(OrganizerDecideRequest req, CancellationToken ct)
    {
        var organizer = await organizerRepository.FindAndEnsureExistenceAsync(req.OrganizerId, ct);
        organizer.Decide(req.IsAccepted);
        await organizerRepository.UpdateAsync(organizer, ct);

        await SendOkAsync(ct);
    }
}
