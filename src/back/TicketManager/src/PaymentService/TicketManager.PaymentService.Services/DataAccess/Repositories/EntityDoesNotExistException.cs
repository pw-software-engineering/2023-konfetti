namespace TicketManager.PaymentService.Services.DataAccess.Repositories;

public class EntityDoesNotExistException : Exception
{
    public EntityDoesNotExistException() : base("Entity does not exist!") {}
}
