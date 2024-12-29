using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos.Files;

namespace P2Project.Application.Volunteers.Commands.AddPetPhotos
{
    public record AddPetPhotosCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files) : ICommand;
}
