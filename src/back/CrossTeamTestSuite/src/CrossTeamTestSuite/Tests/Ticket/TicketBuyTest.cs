using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Tickets;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.Ticket;

public class TicketBuyTest: SingleTest
{
    public override string Name => "Ticket buy test";
    public override async Task ExecuteAsync()
    {
        var ticketBuyRequest = new TicketBuyRequest
        {
            EventId = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Id,
            SectorName = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Sectors.First().Name,
            NumberOfSeats = 4
        };

    }
}
