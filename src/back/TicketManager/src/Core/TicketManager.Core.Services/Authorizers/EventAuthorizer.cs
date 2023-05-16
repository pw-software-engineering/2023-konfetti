using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Authorizers;

public class EventAuthorizer<TRequest> : IPreProcessor<TRequest>
    where TRequest : IEventRelated
{
    private readonly Repository<Event, Guid> events;

    public EventAuthorizer(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public async Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        if (req.Role == AccountRoles.Admin)
        {
            return;
        }

        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);

        if (@event.OrganizerId != req.AccountId)
        {
            await ctx.Response.SendUnauthorizedAsync(ct);
        }
    }
}
