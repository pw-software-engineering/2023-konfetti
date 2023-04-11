using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Contracts.Common;

namespace CrossTeamTestSuite.Endpoints.Contracts.Organizers;

public class OrganizerListRequest : IPaginatedRequest, ISortedRequest<OrganizerListSortByDto>, IRequest<PaginatedResponse<OrganizerDto>>
{
    [JsonIgnore] 
    public string Path => "/organizer/list";
    [JsonIgnore] 
    public RequestType Type => RequestType.Get;
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool ShowAscending { get; set; }
    public OrganizerListSortByDto SortBy { get; set; }
    
    public string? CompanyNameFilter { get; set; }
    public string? AddressFilter { get; set; }
    public string? TaxIdFilter { get; set; }
    public List<TaxIdTypeDto>? TaxIdTypesFilter { get; set; }
    public string? DisplayNameFilter { get; set; }
    public string? EmailFilter { get; set; }
    public List<VerificationStatusDto>? VerificationStatusesFilter { get; set; }
}

public enum OrganizerListSortByDto
{
    DisplayName = 0,
    Email = 1,
    CompanyName = 2,
    Address = 3
}