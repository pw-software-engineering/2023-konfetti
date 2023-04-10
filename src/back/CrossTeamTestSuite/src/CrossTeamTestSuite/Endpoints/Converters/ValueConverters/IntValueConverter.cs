namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class IntValueConverter : IValueConverter<int>
{
    public string Convert(string Name, int value)
    {
        return $"{Name}={value}";
    }
}
