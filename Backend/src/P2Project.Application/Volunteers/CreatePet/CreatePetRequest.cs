using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Species;
using P2Project.Application.Species.Create;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using System.Xml.Linq;

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

    public record CreatePetCommand(
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

    public class CreatePetValidator
    {
        public CreatePetValidator()
        {

        }
    }

    public class CreatePetHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<CreatePetHandler> _petLogger;
        private readonly ILogger<CreateHandler> _speciesLogger;

        public CreatePetHandler(
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            ILogger<CreatePetHandler> petLogger,
            ILogger<CreateHandler> speciesLogger)
        {
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _petLogger = petLogger;
            _speciesLogger = speciesLogger;
        }
        public async Task<Result<Guid, Error>> Handle(
            CreatePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            var petId = PetId.NewPetId();
            var nicKName = NickName.Create(command.NickName).Value;

            var speciesName = Name.Create(command.Species).Value;

            var speciesIfExist = await _speciesRepository.GetByName(
                speciesName, cancellationToken);

            if(speciesIfExist.IsFailure)
            {
                var newBreeds = new List<Breed>();
                if (command.Breed != null)
                {
                    var breed = new Breed(Name.Create(command.Breed).Value);
                    newBreeds.AddRange([breed]);
                }

                var species = new Domain.SpeciesManagment.Species(
                    SpeciesId.NewSpeciesId(), speciesName, newBreeds);

                await _speciesRepository.Add(species, cancellationToken);

                _speciesLogger.LogInformation(
                    "Species created with ID: {id}",
                    species.Id.Value);
            }
            var speciesExist = await _speciesRepository.GetByName(
                speciesName, cancellationToken);
            var speciesId = speciesExist.Value.Id;

            var breedId = speciesExist.Value?.Breeds?.Where(b =>
                b.Name.Value == command.Breed)?.Select(r => r.Id)?.FirstOrDefault();

            if (breedId == null && breedId.Value != Guid.Empty)
                return Errors.General.ValueIsInvalid(command.Breed);

            var speciesBreed = new SpeciesBreed(speciesId, breedId.Value);

            var description = Description.Create(command.Description);
            var color = Color.Create(command.Color);
            var healthInfo = HealthInfo.Create(command.HealthInfo);
            var address = Address.Create(
                command.Address.Region,
                command.Address.City,
                command.Address.Street,
                command.Address.House,
                command.Address.Floor,
                command.Address.Apartment);
            var ownerPhoneNumber = PhoneNumber.Create(
                command.OwnerPhoneNumber.Value,
                command.OwnerPhoneNumber.IsMain);
            var assistanceStatus = AssistanceStatus.Create(
                command.AssistanceStatus);

            var assistanceDetails = new List<AssistanceDetail>();
            if (command.AssistanceDetails != null)
            {
                var details = command.AssistanceDetails.Select(ad =>
                                                        AssistanceDetail.Create(
                                                            ad.Name,
                                                            ad.Description,
                                                            ad.AccountNumber).Value);
                assistanceDetails.AddRange(details);
            }
            var petAssistanceDetails = new VolunteerAssistanceDetails(assistanceDetails);



            var newPet = new Pet();

            volunteerResult.Value.AddPet(newPet);
        }
    }
}
