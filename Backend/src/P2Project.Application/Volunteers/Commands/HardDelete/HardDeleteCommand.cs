using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.HardDelete;

public record HardDeleteCommand(Guid VolunteerId) : ICommand;