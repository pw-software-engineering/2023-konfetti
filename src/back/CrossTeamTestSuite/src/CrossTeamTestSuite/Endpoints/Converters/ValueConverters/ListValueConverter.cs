using System.Text;

namespace CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

public class ListValueConverter<T> : IValueConverter<List<T>>
{
    private readonly IValueConverter<T> itemValueConverter;

    public ListValueConverter(IValueConverter<T> itemValueConverter)
    {
        this.itemValueConverter = itemValueConverter;
    }
    
    public ListValueConverter() :this(new SimpleValueConverter<T>()) 
    { }

    public string Convert(string name, List<T> value)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in value)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append('&');
            }
            stringBuilder.Append(itemValueConverter.Convert(name, item));
        }
        return stringBuilder.ToString();
    }
}
