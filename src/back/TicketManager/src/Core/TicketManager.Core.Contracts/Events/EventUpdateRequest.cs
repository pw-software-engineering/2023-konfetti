namespace TicketManager.Core.Contracts.Events;

public class EventUpdateRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? Date { get; set; }
    public List<SectorDto>? Sectors { get; set; }
    
    public static class ErrorCodes
    {
        public const int NameIsEmpty = 1;
        public const int NameIsTooLong = 2;
        public const int DescriptionIsEmpty = 3;
        public const int DescriptionIsTooLong = 4;
        public const int LocationIsEmpty = 5;
        public const int LocationIsTooLong = 6;
        public const int DateIsNotFuture = 7;
        public const int SectorsAreEmpty = 8;
        public const int SectorNamesAreNotUnique = 9;

        public class SectorErrorCodes : SectorDto.ErrorCodes { }
    }
}
