using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.PdfGenerator.Contracts;
using TicketManager.PdfGenerator.Contracts.Ticket;

namespace TicketManager.Core.Services.Processes.Tickets;

public class CreateTicket
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public Guid SectorId { get; set; }
    public List<TicketSeat> Seats { get; set; } = null!;
}

public class TicketSeat
{
    public int Row { get; set; }
    public int Column { get; set; }
}

public class CreateTicketConsumer : IConsumer<CreateTicket>
{
    private readonly Repository<Ticket, Guid> tickets;
    private readonly IBus bus;
    private readonly CoreDbContext dbContext;

    public CreateTicketConsumer(Repository<Ticket, Guid> tickets, IBus bus, CoreDbContext dbContext)
    {
        this.tickets = tickets;
        this.bus = bus;
        this.dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CreateTicket> context)
    {
        var message = context.Message;
        var ticket = new Ticket(
            message.TicketId,
            message.UserId,
            message.EventId,
            message.SectorId,
            message.Seats.Select(s => new Domain.Tickets.TicketSeat(s.Row, s.Column)));

        await tickets.AddAsync(ticket, context.CancellationToken);

        var sector = await dbContext
            .Sectors
            .Where(s => s.Id == context.Message.SectorId)
            .FirstOrDefaultAsync(context.CancellationToken);
        
        var @event = await dbContext
            .Events
            .Where(e => e.Id == context.Message.EventId)
            .FirstOrDefaultAsync(context.CancellationToken);
        
        var user = await dbContext
            .Users
            .Where(u => u.Id == context.Message.UserId)
            .FirstOrDefaultAsync(context.CancellationToken);
        
        var organizer = await dbContext
            .Organizers
            .Where(o => o.Id == @event!.OrganizerId)
            .FirstOrDefaultAsync(context.CancellationToken);
        
        await bus.Publish(new GenerateTicketPdf
        {
            Ticket = new TicketDto
            {
                Id = ticket.Id,
                SectorName = sector!.Name,
                Seats = ticket.Seats.Select(s => new TicketSeatDto
                {
                    Row = s.Row,
                    Column = s.Column
                }).ToList()
            },
            User = new UserDto
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate.ToDateTime(TimeOnly.MinValue)
            },
            Event = new EventDto
            {
                Name = @event!.Name,
                Date = @event.Date,
                Description = @event.Description,
                Location = @event.Location,
                Organizer = new OrganizerDto
                {
                    DisplayName = organizer!.DisplayName,
                    Address = organizer.Address,
                    PhoneNumber = organizer.PhoneNumber
                }
            }
        });
    }
}
