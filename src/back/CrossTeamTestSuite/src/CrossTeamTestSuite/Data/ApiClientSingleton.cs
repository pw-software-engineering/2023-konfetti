using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace CrossTeamTestSuite.Data;

public class ApiClientSingleton
{
    private static HttpClient? httpClient;

    public static void ConfigureClient(string address)
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(address);
    }

    public static HttpClient GetInstance()
    {
        if (httpClient is null)
        {
            throw new SettingsException("HttpClient is not configured.");
        }

        return httpClient;
    }
}
