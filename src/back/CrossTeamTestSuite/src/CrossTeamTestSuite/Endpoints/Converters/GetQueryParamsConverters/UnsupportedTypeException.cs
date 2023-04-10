namespace CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;

public class UnsupportedTypeException : Exception
{
    public UnsupportedTypeException() : base("Unsupported type found") { }
}
