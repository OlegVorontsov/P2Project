using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.SoftDelete
{
    public record SoftDeleteCommand(Guid VolunteerId) : ICommand;
}
