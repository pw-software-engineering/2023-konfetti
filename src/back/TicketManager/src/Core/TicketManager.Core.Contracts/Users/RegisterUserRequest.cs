namespace TicketManager.Core.Contracts.Users;

public class RegisterUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }

    public static class ErrorCodes
    {
        public const int EmailIsEmpty = 1;
        public const int EmailIsTooLong = 2;
        public const int EmailIsAlreadyTaken = 3;
        public const int EmailIsInvalid = 4;
        public const int PasswordIsEmpty = 5;
        public const int PasswordIsTooLong = 6;
        public const int PasswordIsInvalid = 7;
        public const int FirstNameIsEmpty = 8;
        public const int FirstNameIsTooLong = 9;
        public const int LastNameIsEmpty = 10;
        public const int LastNameIsTooLong = 11;
    }
}
