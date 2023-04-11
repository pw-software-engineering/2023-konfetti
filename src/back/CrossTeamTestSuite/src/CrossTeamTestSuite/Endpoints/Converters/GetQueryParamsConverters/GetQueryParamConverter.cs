using System.Reflection;
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
            new StringValueConverter(),
            new SimpleValueConverter<int>(),
            new ListValueConverter<int>(),
        };
    }

    public string GetParams(TRequest request)
    {
        var resultBuilder = new StringBuilder();
        var properties = GetProperties();
        foreach (var p in properties)
        {
            AppendProperty(request, p, resultBuilder);
        }

        if (resultBuilder.Length > 0)
        {
            resultBuilder.Insert(0, '?');
        }
        
        return resultBuilder.ToString();
    }

    private void AppendProperty(TRequest request, PropertyInfo p, StringBuilder resultBuilder)
    {
        var valueConverter = GetValueConverter(p);
        var conversionResult = GetConversionResult(request, valueConverter, p);
        AppendConversionResult(resultBuilder, conversionResult);
    }

    private static void AppendConversionResult(StringBuilder resultBuilder, object? conversionResult)
    {
        if (resultBuilder.Length > 0)
        {
            resultBuilder.Append('&');
        }

        resultBuilder.Append(conversionResult);
    }

    private static object? GetConversionResult(TRequest request, object valueConverter, PropertyInfo p)
    {
        var convertMethod = valueConverter.GetType().GetMethods().First(m => m.Name == "Convert");
        var conversionResult = convertMethod.Invoke(valueConverter, new[] { p.Name, p.GetValue(request) });
        return conversionResult;
    }

    private object GetValueConverter(PropertyInfo p)
    {
        var valueConverter = valueConverters
            .FirstOrDefault(vc => vc
                .GetType()
                .GetMethods()
                .First(m => m.Name == "Convert")
                .GetParameters()
                .Any(mp => mp.ParameterType == p.PropertyType));

        if (valueConverter is null)
        {
            throw new UnsupportedTypeException();
        }

        return valueConverter;
    }

    private static List<PropertyInfo> GetProperties()
    {
        return typeof(TRequest)
            .GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).FirstOrDefault() is null)
            .ToList();
    }
}
