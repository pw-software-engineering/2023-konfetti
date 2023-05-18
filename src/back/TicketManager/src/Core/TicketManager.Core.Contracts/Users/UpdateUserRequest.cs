using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Users;

public class UpdateUserRequest
{
    [FromClaim(Claims.AccountId)]
    public Guid AccountId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    
    public static class ErrorCodes
    {
        public const int EmailIsEmpty = 1;
        public const int EmailIsTooLong = 2;
        public const int EmailIsAlreadyTaken = 3;
        public const int EmailIsInvalid = 4;
        public const int FirstNameIsEmpty = 5;
        public const int FirstNameIsTooLong = 6;
        public const int LastNameIsEmpty = 7;
        public const int LastNameIsTooLong = 8;
    }
}
