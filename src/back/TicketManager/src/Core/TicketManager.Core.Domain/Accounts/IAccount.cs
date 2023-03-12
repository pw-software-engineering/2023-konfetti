namespace TicketManager.Core.Domain.Accounts;

public interface IAccount
{
    public Guid Id { get; }
    public string Email { get; }
}

public static class IAccountExtensions
{
    public static Account GetAccount(this IAccount account, string passwordHash)
    {
        return new Account(account.Id, account.Email, passwordHash);
    }
}
