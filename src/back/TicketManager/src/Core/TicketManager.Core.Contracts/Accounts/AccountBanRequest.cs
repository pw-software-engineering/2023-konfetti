namespace TicketManager.Core.Contracts.Accounts;

public class AccountBanRequest
{
    public Guid AccountId { get; set; }

    public static class ErrorCodes
    {
        public const int AccountDoesNotExist = 1;
    }
}
