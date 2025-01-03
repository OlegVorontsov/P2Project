using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;