namespace CrossTeamTestSuite.Data;

public class CommonTokenSingleton
{
    private static string adminToken = string.Empty;
    private static string organizerToken = string.Empty;
    private static string userToken = string.Empty;

    
    public static string GetToken(CommonTokenType tokenType)
    {
        return tokenType switch
        {
            CommonTokenType.AdminToken => adminToken,
            CommonTokenType.OrganizerToken => organizerToken,
            CommonTokenType.UserToken => userToken,
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };
    }
    
    public static void SetToken(CommonTokenType tokenType, string token)
    {
        switch (tokenType)
        {
            case CommonTokenType.AdminToken:
                adminToken = token;
                break;
            case CommonTokenType.OrganizerToken:
                organizerToken = token;
                break;
            case CommonTokenType.UserToken:
                userToken = token;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null);
        }
    }

}

public enum CommonTokenType
{
    AdminToken = 0,
    OrganizerToken = 1,
    UserToken = 2
}
