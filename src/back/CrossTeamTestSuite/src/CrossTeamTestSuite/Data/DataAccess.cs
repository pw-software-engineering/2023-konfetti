using CrossTeamTestSuite.Data.Repository;
using CrossTeamTestSuite.Data.Repository.Users;

namespace CrossTeamTestSuite.Data;

public class DataAccess
{
    public UserRepository UserRepository { get; private init; }
    
    public DataAccess()
    {
        UserRepository = new UserRepository();
    }
}
