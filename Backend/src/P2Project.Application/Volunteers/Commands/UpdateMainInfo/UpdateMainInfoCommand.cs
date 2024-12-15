using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
                  Guid VolunteerId,
                  FullNameDto FullName,
                  int Age,
                  string Gender,
                  string? Description) : ICommand;
}
