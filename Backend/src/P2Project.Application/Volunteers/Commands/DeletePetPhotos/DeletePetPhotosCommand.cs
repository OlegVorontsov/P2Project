using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;