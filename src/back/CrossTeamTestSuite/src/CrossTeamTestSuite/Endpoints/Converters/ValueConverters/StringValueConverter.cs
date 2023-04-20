using System.Web;

namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class StringValueConverter : IValueConverter<string>
{
    public string Convert(string name, string value)
    {
        return $"{name}={HttpUtility.UrlPathEncode(value)}";
    }
}
