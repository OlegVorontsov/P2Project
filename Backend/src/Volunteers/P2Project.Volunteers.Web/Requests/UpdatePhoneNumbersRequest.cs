using P2Project.Core.Dtos.Common;
using P2Project.Volunteers.Application.Commands.UpdatePhoneNumbers;

namespace P2Project.Volunteers.Web.Requests;

public record UpdatePhoneNumbersRequest(
    IEnumerable<PhoneNumberDto> PhoneNumbers)
{
    public UpdatePhoneNumbersCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, PhoneNumbers);
}
