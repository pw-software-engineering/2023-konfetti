using System.Linq.Expressions;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;

namespace TicketManager.Core.Services.DataAccess.DtoMappers;

public class EventDtoMapper
{
    public static readonly Func<(Event Event, IEnumerable<Sector> Sectors), EventDto> ToDtoMapper = t => new EventDto
    {
        Id = t.Event.Id,
        OrganizerId = t.Event.OrganizerId,
        Name = t.Event.Name,
        Description = t.Event.Description,
        Location = t.Event.Location,
        Date = t.Event.Date,
        Sectors = t.Sectors.Select(s => new SectorDto
        {
            Name = s.Name,
            PriceInSmallestUnit = s.PriceInSmallestUnit,
            NumberOfColumns = s.NumberOfColumns,
            NumberOfRows = s.NumberOfRows
        }).ToList()
    };
}
