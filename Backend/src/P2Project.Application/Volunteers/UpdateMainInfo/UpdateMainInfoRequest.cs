using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(
              FullNameDto FullName,
              int Age,
              string Gender,
              string? Description)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new (volunteerId, FullName, Age, Gender, Description);
}
