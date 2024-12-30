using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.HardDeletePet;

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;