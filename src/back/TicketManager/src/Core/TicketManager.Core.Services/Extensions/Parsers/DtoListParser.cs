using FastEndpoints;

namespace TicketManager.Core.Services.Extensions.Parsers;

public class DtoListParser<T> where T: struct, Enum
{
    public static ParseResult Parse(object? input)
    {
        try
        {
            var inputAsString = input!.ToString();
            var parts = inputAsString!.Split(',');
            var result = parts.Select(p =>
                {
                    var success = Enum.TryParse<T>(p, out var val);
                    return (success, val);
                })
                .ToList();
            return new(result.All(r => r.success), result.Select(r => r.val).ToList());
        }
        catch (Exception)
        {
            return new(false, new List<T>());
        }
    }
}
