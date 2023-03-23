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
        public const int PasswordIsNull = 5;
        public const int PasswordIsTooShort = 6;
        public const int PasswordIsTooLong = 7;
        public const int PasswordIsInvalid = 8;
        public const int FirstNameIsEmpty = 9;
        public const int FirstNameIsTooLong = 10;
        public const int LastNameIsEmpty = 11;
        public const int LastNameIsTooLong = 12;
    }
}
