using System.Linq.Expressions;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Organizer;

namespace TicketManager.Core.Services.DataAccess.DtoMappers;

public static class OrganizerDtoMapper
{
    public readonly static Expression<Func<Organizer, OrganizerDto>> ToDtoMapper = o => new OrganizerDto
    {
        Id = o.Id,
        Address = o.Address,
        CompanyName = o.CompanyName,
        DisplayName = o.DisplayName,
        Email = o.Email,
        PhoneNumber = o.PhoneNumber,
        TaxIdType = (TaxIdTypeDto)o.TaxIdType,
        TaxId = o.TaxId,
        VerificationStatus = VerificationStatusDto.VerifiedPositively, // TODO: fix it
    };
}
