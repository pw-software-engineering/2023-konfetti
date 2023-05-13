using System.Linq.Expressions;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Helpers;

public static class EventHelpers
{
    
    public static IOrderedQueryable<Event> HandleEventFilter(this IQueryable<Event> query, IEventListRequest req)
    {
        query = query
            .FilterStringField(e => e.Location, req.Location)
            .FilterStringField(e => e.Name, req.EventNameFilter)
            .FilterDateTimeField(e => e.Date, req.EarlierThanInclusiveFilter, DateTimeFilterType.EarlierThanInclusive)
            .FilterDateTimeField(e => e.Date, req.LaterThanInclusiveFilter, DateTimeFilterType.LaterThanInclusive)
            .FilterListField(e => e.Status, req.EventStatusesFilter);
        
        
        Expression<Func<Event, object>> sortExpression = req.SortBy switch
        {
            EventListSortByDto.Location => (e) => e.Location,
            EventListSortByDto.Date => (e) => e.Date,
            _ => (e) => e.Id
        };
        return query.OrderBy(sortExpression);
    }
}
