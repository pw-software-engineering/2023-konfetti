namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class StringValueConverter : IValueConverter<string>
{
    public string Convert(string Name, string value)
    {
        return $"{Name}={value}";
    }
}
