using MassTransit;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Events;

public class DeleteSectorReservation
{
    public Guid SectorId { get; set; }
}

public class DeleteSectorReservationConsumer: IConsumer<DeleteSectorReservation>
{
    private readonly Repository<Sector, Guid> sectors;

    public DeleteSectorReservationConsumer(Repository<Sector, Guid> sectors)
    {
        this.sectors = sectors;
    }

    public async Task Consume(ConsumeContext<DeleteSectorReservation> context)
    {
        var sector = await sectors.FindAndEnsureExistenceAsync(context.Message.SectorId, context.CancellationToken);
        sector.RemoveAllReservations();
        sector.RemoveAllTakenSeats();
        await sectors.UpdateAsync(sector, context.CancellationToken);
    }
}
