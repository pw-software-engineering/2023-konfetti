using System.Linq.Expressions;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;

namespace TicketManager.Core.Services.DataAccess.DtoMappers;

public class EventDtoMapper
{
    public static readonly Expression<Func<Tuple<Event, IEnumerable<Sector>>, EventDto>> ToDtoMapper = t => new EventDto
    {
        Id = t.Item1.Id,
        OrganizerId = t.Item1.OrganizerId,
        Name = t.Item1.Name,
        Description = t.Item1.Description,
        Location = t.Item1.Location,
        Date = t.Item1.Date,
        Sectors = t.Item2.Select(s => new SectorDto
        {
            Name = s.Name,
            PriceInSmallestUnit = s.PriceInSmallestUnit,
            NumberOfColumns = s.NumberOfColumns,
            NumberOfRows = s.NumberOfRows
        }).ToList(),
    };
}
