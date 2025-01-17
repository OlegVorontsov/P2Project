using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.SoftDeletePet;

public record SoftDeletePetCommand(
    Guid VolunteerId, Guid PetId) : ICommand;