using P2Project.Core.Dtos.Files;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public record AddPetPhotosCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files) : ICommand;
}
