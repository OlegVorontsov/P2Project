using FluentValidation;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;

namespace P2Project.Application.Volunteers.UpdateAssistanceDetails
{
    public record UpdateAssistanceDetailsRequest(
        Guid VolunteerId,
        UpdateAssistanceDetailsDto AssistanceDetailsDto);

    public record UpdateAssistanceDetailsDto(
        IEnumerable<AssistanceDetailDto> AssistanceDetails);
}
