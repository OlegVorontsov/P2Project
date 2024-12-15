using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.Delete
{
    public record DeleteCommand(Guid VolunteerId) : ICommand;
}
