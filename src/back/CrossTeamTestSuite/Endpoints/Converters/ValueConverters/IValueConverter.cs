namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public interface IValueConverter<T>
{
    public string Convert(string Name, T value);
}
