namespace CrossTeamTestSuite.Data.Repository.Organizers;

public class OrganizerRepository: IAccountRepository
{
    private List<Organizer> organizers = new();
    public string DefaultEmail { get; } = "organizer.ctts@email.com";
    public string DefaultPassword { get; } = "Password1";
    public IReadOnlyList<Organizer> Organizers => organizers.AsReadOnly();
    
    public void AddOrganizer(Organizer organizer)
    {
        organizers.Add(organizer);
    }
}
