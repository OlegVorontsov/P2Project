using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UploadFilesToPet
{
    public record UploadFilesToPetCommand(
        Guid VolunteerId,
        Guid PetId,
        IEnumerable<UploadFileDto> Files);
}
