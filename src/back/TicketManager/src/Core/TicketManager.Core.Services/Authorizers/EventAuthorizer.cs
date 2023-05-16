using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Authorizers;

public class EventAuthorizer<TRequest> : IPreProcessor<TRequest>
    where TRequest : IEventRelated
{
    private readonly IServiceScopeFactory scopeFactory;

    public EventAuthorizer(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public async Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        if (ctx.User.IsInRole(AccountRoles.Admin))
        {
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var events = scope.ServiceProvider.GetRequiredService<Repository<Event, Guid>>();

        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);
        
        if (@event.OrganizerId != req.AccountId)
        {
            await ctx.Response.SendUnauthorizedAsync(ct);
        }
    }
}
