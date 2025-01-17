using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;