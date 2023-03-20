namespace TicketManager.Core.Contracts.Organizers;

public class RegisterOrganizerRequest
{
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string TaxId { get; set; }
    public TaxIdTypeDto TaxIdType { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
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
        public const int CompanyNameIsEmpty = 9;
        public const int CompanyNameIsTooLong = 10;
        public const int AddressIsEmpty = 11;
        public const int AddressIsTooLong = 12;
        public const int TaxIdIsEmpty = 13;
        public const int TaxIdIsTooLong = 14;
        public const int DisplayNameIsEmpty = 15;
        public const int DisplayNameIsTooLong = 16;
        public const int PhoneNumberIsEmpty = 17;
        public const int PhoneNumberIsTooLong = 18;
    }
}

public enum TaxIdTypeDto
{
    Nip = 0,
    Regon = 1,
    Krs = 2,
    Pesel = 3,
    Vatin = 4
}
