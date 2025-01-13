using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.HardDelete;

public record HardDeleteCommand(Guid VolunteerId) : ICommand;