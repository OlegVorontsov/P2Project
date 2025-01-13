using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.HardDeletePet;

public record HardDeletePetCommand(
    Guid VolunteerId, Guid PetId) : ICommand;