using CrossTeamTestSuite.Data.Repository;
using CrossTeamTestSuite.Data.Repository.Organizers;
using CrossTeamTestSuite.Data.Repository.Users;

namespace CrossTeamTestSuite.Data;

public class DataAccess
{
    public UserRepository UserRepository { get; private init; }
    public OrganizerRepository OrganizerRepository { get; private init; }
    
    public DataAccess()
    {
        UserRepository = new UserRepository();
        OrganizerRepository = new OrganizerRepository();
    }
}
