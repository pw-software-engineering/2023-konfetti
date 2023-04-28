using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Accounts;

public class AccountUpdatePasswordRequest
{
    [FromClaim(Claims.AccountId)]
    public Guid AccountId { get; set; }
    public string Password { get; set; }

    public static class ErrorCodes
    {
        public const int PasswordIsNull = 0;
        public const int PasswordIsTooShort = 1;
        public const int PasswordIsTooLong = 2;
        public const int PasswordIsInvalid = 3;
    }
}
