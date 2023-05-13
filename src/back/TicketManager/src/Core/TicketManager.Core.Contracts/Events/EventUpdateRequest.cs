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
        public const int NameIsTooLong = 1;
        public const int DescriptionIsTooLong = 2;
        public const int LocationIsTooLong = 3;
        public const int DateIsNotFuture = 4;
        public const int SectorNamesAreNotUnique = 5;

        public class SectorErrorCodes : SectorDto.ErrorCodes { }
    }
}
