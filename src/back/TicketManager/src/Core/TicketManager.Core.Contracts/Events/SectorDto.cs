namespace TicketManager.Core.Contracts.Events;

public class SectorDto
{
    public string Name { get; set; }
    public int PriceInSmallestUnit { get; set; }
    public int NumberOfColumns { get; set; }
    public int NumberOfRows { get; set; }

    public class ErrorCodes
    {
        public const int NameIsEmpty = 1_001;
        public const int NameIsTooLong = 1_002;
        public const int PriceInSmallestUnitIsNotPositive = 1_003;
        public const int NumberOfColumnsIsNotPositive = 1_004;
        public const int NumberOfRowsIsNotPositive = 1_005;
    }
}
