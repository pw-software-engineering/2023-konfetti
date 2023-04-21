namespace CrossTeamTestSuite.Data;

public class DataAccessSingleton
{
    private static DataAccess? dataAccess;
    
    public static DataAccess GetInstance()
    {
        if (dataAccess is null)
        {
            dataAccess = new DataAccess();
        }
        return dataAccess;
    }
}
