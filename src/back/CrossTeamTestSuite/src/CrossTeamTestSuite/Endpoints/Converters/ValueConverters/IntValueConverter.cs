namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class IntValueConverter : IValueConverter<int>
{
    public string Convert(string name, int value)
    {
        return $"{name}={value}";
    }
}
