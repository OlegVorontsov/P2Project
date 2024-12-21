using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Files;

namespace P2Project.Application.Volunteers.Commands.UploadFilesToPet
{
    public record UploadFilesToPetCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files) : ICommand;
}
