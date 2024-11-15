using P2Project.Application.Dtos;

namespace P2Project.Application.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoRequest(
                  Guid VolunteerId,
                  UpdateMainInfoDto MainInfoDto);

    public record UpdateMainInfoDto(
              FullNameDto FullName,
              int Age,
              string Gender,
              string? Description);
}
