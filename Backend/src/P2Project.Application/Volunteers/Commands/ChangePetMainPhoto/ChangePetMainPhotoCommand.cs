using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Volunteers.Commands.ChangePetMainPhoto;

public record ChangePetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string ObjectName) : ICommand;