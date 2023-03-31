using System.Linq.Expressions;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;

namespace TicketManager.Core.Services.DataAccess.DtoMappers;

public class EventDtoMapper
{
    public readonly static Expression<Func<Event, EventDto>> ToDtoMapper = e => new EventDto
    {
        Id = e.Id,
        OrganizerId = e.OrganizerId,
        Name = e.Name,
        Description = e.Description,
        Location = e.Location,
        Date = e.Date,
        Sectors = e.Sectors
            .Select(s => new SectorDto
            {
                Name = s.Name,
                PriceInSmallestUnit = s.PriceInSmallestUnit,
                NumberOfColumns = s.NumberOfColumns,
                NumberOfRows = s.NumberOfRows,
            })
            .ToList(),
    };
}
