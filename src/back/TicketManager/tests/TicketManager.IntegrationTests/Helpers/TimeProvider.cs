namespace TicketManager.IntegrationTests.Helpers;

public static class TimeProvider
{
    public static DateTime Now()
    {
        var result = DateTime.UtcNow;
        return new DateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, DateTimeKind.Utc);
    }
}
