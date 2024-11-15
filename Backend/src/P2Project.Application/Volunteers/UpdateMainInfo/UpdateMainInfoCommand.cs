using P2Project.Application.Dtos;

namespace P2Project.Application.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
                  Guid VolunteerId,
                  FullNameDto FullName,
                  int Age,
                  string Gender,
                  string? Description);
}
