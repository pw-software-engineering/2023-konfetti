namespace TicketManager.Core.Services.Extensions;

public static class StringExtensions
{
    public static bool ContainsCaseInsensitive(this string s, string compare)
    {
        return s.ToLower().Contains(compare);
    }
}
