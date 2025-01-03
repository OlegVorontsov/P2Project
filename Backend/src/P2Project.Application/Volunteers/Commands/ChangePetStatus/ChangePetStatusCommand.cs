using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.ChangePetStatus;

public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string Status) : ICommand;