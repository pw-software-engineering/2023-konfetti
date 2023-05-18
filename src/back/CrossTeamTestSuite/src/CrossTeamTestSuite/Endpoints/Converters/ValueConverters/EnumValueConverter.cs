namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class EnumValueConverter<T> : IValueConverter<T>
    where T : Enum
{
    public string Convert(string name, T value)
    {
        return $"{name}={(int)(object)value}";
    }
}
