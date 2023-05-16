using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Organizers;

public class UpdateOrganizerRequest
{
    [FromClaim(Claims.AccountId)]
    public Guid AccountId { get; set; }
    public string? CompanyName { get; set; }
    public string? Address { get; set; }
    public string? TaxId { get; set; }
    public TaxIdTypeDto? TaxIdType { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public static class ErrorCodes
    {
        public const int EmailIsEmpty = 1;
        public const int EmailIsTooLong = 2;
        public const int EmailIsAlreadyTakenByAnotherAccount = 3;
        public const int EmailIsInvalid = 4;
        public const int CompanyNameIsEmpty = 5;
        public const int CompanyNameIsTooLong = 6;
        public const int AddressIsEmpty = 7;
        public const int AddressIsTooLong = 8;
        public const int TaxIdIsEmpty = 9;
        public const int TaxIdIsTooLong = 10;
        public const int TaxIdTypeIsInvalid = 11;
        public const int DisplayNameIsEmpty = 12;
        public const int DisplayNameIsTooLong = 13;
        public const int PhoneNumberIsEmpty = 14;
        public const int PhoneNumberIsTooLong = 15;
    }
}
