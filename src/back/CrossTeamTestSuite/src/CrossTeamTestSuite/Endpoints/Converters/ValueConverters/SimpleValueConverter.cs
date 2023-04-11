namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class SimpleValueConverter<T> : IValueConverter<T>
{
    public string Convert(string name, T value)
    {
        return $"{name}={value}";
    }
}
