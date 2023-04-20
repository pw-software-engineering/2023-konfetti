namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class DateOnlyValueConverter: IValueConverter<DateOnly>
{
    public string Convert(string name, DateOnly value)
    {
        return $"{name}={value:yyyy-MM-dd}";
    }
}
