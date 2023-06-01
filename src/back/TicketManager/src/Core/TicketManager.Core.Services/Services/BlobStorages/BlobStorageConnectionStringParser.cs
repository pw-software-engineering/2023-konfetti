namespace TicketManager.Core.Services.Services.BlobStorages;

public static class BlobStorageConnectionStringParser
{
    public static Dictionary<string, string> Parse(string connectionString)
    {
        var result = new Dictionary<string, string>();

        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var p = part.Split('=');
            var key = p[0];
            var value = String.Join("=", p.Skip(1));
            result.Add(key, value);
        }
        
        return result;
    }
}
