using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Services.Extensions;

public static class QueryableExtensions
{
    private const int MaxPageSize = 100;
    
    public static async Task<PaginatedResponse<TDto>> ToPaginatedResponseAsync<TDto, TReq>(this IQueryable<TDto> query, TReq req, CancellationToken ct)
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
        Expression<Func<T, TKey>> exp, ISortedRequest<TSort> req)
        where TSort : Enum
    {
        return req.ShowAscending ? query.OrderBy(exp) : query.OrderByDescending(exp);
    }

    public static IQueryable<T> FilterStringField<T>(this IQueryable<T> query, Expression<Func<T, string>> exp,
        string? filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
        {
            return query;
        }
        var valueToLower = filterValue.ToLower();
        var methodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var methodLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        if (methodContains is null || methodLower is null)
        {
            return query;
        }

        var loweredExpression = Expression.Call(exp.Body, methodLower);
        var call = Expression.Call(loweredExpression, methodContains, Expression.Constant(valueToLower));
        var lambda = Expression.Lambda<Func<T, bool>>(call, exp.Parameters);
        return query.Where(lambda);
    }
    
    public static IQueryable<T> FilterListField<T, TVal, TValDto>(this IQueryable<T> query, Expression<Func<T, TVal>> exp,
        List<TValDto>? filterList)
        where TVal: Enum
        where TValDto: Enum
    {
        if (filterList is null || !filterList.Any())
        {
            return query;
        }
        var methodContains = typeof(List<TValDto>).GetMethod("Contains", new[] { typeof(TValDto) });

        if (methodContains is null)
        {
            return query;
        }
        var castExp = Expression.Convert(exp.Body, typeof(TValDto));
        var call = Expression.Call(Expression.Constant(filterList), methodContains, castExp);
        var lambda = Expression.Lambda<Func<T, bool>>(call, exp.Parameters);
        return query.Where(lambda);
    }
}
