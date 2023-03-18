using FastEndpoints;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.PasswordManagers;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class RegisterOrganizerEndpoint: Endpoint<RegisterOrganizerRequest>
{
    private readonly Repository<Organizer, Guid> organizers;
    private readonly Repository<Account, Guid> accounts;
    private readonly CoreDbContext dbContext;
    private readonly PasswordManager passwordManager;

    public RegisterOrganizerEndpoint(Repository<Organizer, Guid> organizers, Repository<Account, Guid> accounts, CoreDbContext dbContext, PasswordManager passwordManager)
    {
        this.organizers = organizers;
        this.accounts = accounts;
        this.dbContext = dbContext;
        this.passwordManager = passwordManager;
    }

    public override void Configure()
    {
        Post("/organizer/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterOrganizerRequest req, CancellationToken ct)
    {
        var user = new Organizer(req.Email, req.CompanyName, req.Address, req.TaxId, req.TaxIdType, req.DisplayName, req.PhoneNumber);
        var account = user.GetAccount(passwordManager.GetHash(req.Password));

        organizers.Add(user);
        accounts.Add(account);
        
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}