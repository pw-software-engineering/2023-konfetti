using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;

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

    public static IOrderedQueryable<T> SortBy<T, TKey, TSort>(this IQueryable<T> query,
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

    public static IQueryable<T> FilterDateTimeField<T>(this IQueryable<T> query, Expression<Func<T, DateTime>> exp,
        DateTime? filterValue, DateTimeFilterType filterType)
    {
        if (filterValue is null)
        {
            return query;
        }

        filterValue = filterValue.Value.SetKindUtc();
        var comparisonExpression = GetComparisonExpression(exp.Body, (DateTime)filterValue, filterType);
        var lambda = Expression.Lambda<Func<T, bool>>(comparisonExpression, exp.Parameters);
        return query.Where(lambda);
    }

    private static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
    private static Expression GetComparisonExpression(Expression memberExpression, DateTime filterValue, DateTimeFilterType filterType)
    {
        Expression comparisonExpression;
        switch (filterType)
        {
            case DateTimeFilterType.LaterThan:
                comparisonExpression = Expression.GreaterThan(memberExpression, Expression.Constant(filterValue, typeof(DateTime)));
                break;
            case DateTimeFilterType.LaterThanInclusive:
                comparisonExpression = Expression.GreaterThanOrEqual(memberExpression, Expression.Constant(filterValue, typeof(DateTime)));
                break;
            case DateTimeFilterType.EarlierThan:
                comparisonExpression = Expression.LessThan(memberExpression, Expression.Constant(filterValue, typeof(DateTime)));
                break;
            case DateTimeFilterType.EarlierThanInclusive:
                comparisonExpression = Expression.LessThanOrEqual(memberExpression, Expression.Constant(filterValue, typeof(DateTime)));
                break;
            default:
                throw new ArgumentException("Invalid comparison operator.");
        }
        return comparisonExpression;
    }
}

public enum DateTimeFilterType
{
    EarlierThan = 0,
    EarlierThanInclusive = 1,
    LaterThan = 2,
    LaterThanInclusive = 3,
}
