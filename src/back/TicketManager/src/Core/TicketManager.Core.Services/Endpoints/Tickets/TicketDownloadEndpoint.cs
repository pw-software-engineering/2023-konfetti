using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Services.BlobStorages;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketDownloadEndpoint : Endpoint<TicketDownloadRequest, TicketDownloadResponse>
{
    private readonly CoreDbContext dbContext;
    private readonly BlobStorage blobStorage;

    public TicketDownloadEndpoint(CoreDbContext dbContext, BlobStorage blobStorage)
    {
        this.dbContext = dbContext;
        this.blobStorage = blobStorage;
    }

    public override void Configure()
    {
        Get("/ticket/download");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketDownloadRequest req, CancellationToken ct)
    {
        var ticket = await dbContext
            .Tickets
            .FirstOrDefaultAsync(t => t.Id == req.Id, ct);

        if (ticket is null || !ticket.IsPdfGenerated)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var result = new TicketDownloadResponse()
        {
            DownloadUri = blobStorage.GetBlobReadAccessLink(blobStorage.GetContainerClient(), $"ticket-{ticket.Id}.pdf"),
        };

        await SendOkAsync(result, ct);
    }
}
