using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.ChangePetStatus;

public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string Status) : ICommand;