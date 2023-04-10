using System.Text;
using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.ValueConverters;

namespace CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;

public class GetQueryParamConverter<TRequest>
    where TRequest : class, IRequest
{
    private readonly List<object> valueConverters;

    public GetQueryParamConverter()
    {
        valueConverters = new()
        {
            new IntValueConverter(),
        };
    }

    public string GetParams(TRequest request)
    {
        var sb = new StringBuilder();
        var properties = typeof(TRequest)
            .GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).FirstOrDefault() is null)
            .ToList();
        foreach (var p in properties)
        {
            var valueConverter = valueConverters
                .FirstOrDefault(vc => vc
                    .GetType()
                    .GetMethods()
                    .First(m => m.Name == "Convert")
                    .GetParameters()
                    .Any(mp => mp.GetType() == p.GetType()));

            if (valueConverter is null)
            {
                throw new UnsupportedTypeException();
            }

            var convertMethod = valueConverter.GetType().GetMethods().First(m => m.Name == "Convert");
            var conversionResult = convertMethod.Invoke(valueConverter, new[] { p.GetValue(request) });
            sb.Append(conversionResult);
        }

        if (sb.Length > 0)
        {
            sb.Insert(0, '?');
        }
        
        return sb.ToString();
    }
}
