namespace TicketManager.Core.Contracts.Common;

public interface ISortedRequest<T> where T: Enum
{
    public bool ShowAscending { get; set; }
    public T SortBy { get; set; }
}
