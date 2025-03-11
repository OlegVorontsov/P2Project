using FilesService.Core.Dtos;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.AddPetPhotos
{
    public record AddPetPhotosCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files) : ICommand;
}
