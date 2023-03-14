namespace TicketManager.Core.Domain.Accounts;

public interface IAccount
{
    public Guid Id { get; }
    public string Email { get; }

    public Account GetAccount(string passwordHash);
}
