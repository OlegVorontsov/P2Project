using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using P2Project.Application.Shared.Dtos;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.CreatePet
{
    public record CreatePetDto(
        string NickName,
        string Species,
        string Breed,
        string? Description,
        string Color,
        string? HealthInfo,
        AddressDto Address,
        double Weight,
        double Height,
        PhoneNumberDto OwnerPhoneNumber,
        bool IsCastrated,
        bool IsVaccinated,
        DateOnly DateOfBirth,
        string AssistanceStatus,
        IEnumerable<AssistanceDetailDto>? AssistanceDetails,
        IFormFileCollection PetPhotos);

    public record CreatePetRequest(
        Guid VolunteerId,
        string NickName,
        string Species,
        string Breed,
        string? Description,
        string Color,
        string? HealthInfo,
        AddressDto Address,
        double Weight,
        double Height,
        PhoneNumberDto OwnerPhoneNumber,
        bool IsCastrated,
        bool IsVaccinated,
        DateOnly DateOfBirth,
        string AssistanceStatus,
        IEnumerable<AssistanceDetailDto>? AssistanceDetails,
        IEnumerable<FileDto> PetPhotos);

    public record FileDto(string FileName);

    public record CreatePetCommand(CreatePetRequest Request);

    public class CreatePetValidator
    {
        public CreatePetValidator()
        {

        }
    }

    public class CreatePetHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<CreatePetHandler> _logger;

        public CreatePetHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<CreatePetHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            CreatePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.Request.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(command.Request.VolunteerId);

            var newPet = new Pet();

            volunteerResult.Value.AddPet(newPet);
        }
    }
}
