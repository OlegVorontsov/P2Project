using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SoftDelete
{
    public record SoftDeleteCommand(Guid VolunteerId) : ICommand;
}
