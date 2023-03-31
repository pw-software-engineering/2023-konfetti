using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Services.Extensions;

public static class QueryableExtensions
{
    private const int MaxPageSize = 100;
    
    public async static Task<PaginatedResponse<TDto>> ToPaginatedResponseAsync<TDto, TReq>(this IQueryable<TDto> query, TReq req, CancellationToken ct)
        where TDto : class
        where TReq : IPaginatedRequest
    {
        int pageSize = Math.Clamp(req.PageSize, 0, MaxPageSize);
        int pageNumber = req.PageNumber;
        int skip = pageSize * pageNumber;
        
        var count = await query.CountAsync(ct);
        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return new PaginatedResponse<TDto>
        {
            Items = items,
            TotalCount = count,
        };
    }

    public static IQueryable<T> ToOrderedQueryable<T, TKey, TSort>(this IQueryable<T> query, 
        Expression<Func<T, TKey >> exp, ISortedRequest<TSort> req) 
        where TSort : Enum
    {
        return req.ShowAscending ? query.OrderBy(exp) : query.OrderByDescending(exp);
    }
}
