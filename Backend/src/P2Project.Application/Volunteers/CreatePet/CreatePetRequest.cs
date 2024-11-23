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
        AssistanceDetailDto? AssistanceDetail,
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
        AssistanceDetailDto? AssistanceDetail,
        IEnumerable<PetPhotoDto> PetPhotos);

    public record PetPhotoDto(string FileName, bool IsMain);

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
        AssistanceDetailDto AssistanceDetail,
        IEnumerable<PetPhotoDto> PetPhotos);

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
                b.Name.Value == command.Breed)?.Select(r => r.Id)?
                .FirstOrDefault();

            if (breedId == null && breedId.Value != Guid.Empty)
                return Errors.General.ValueIsInvalid(command.Breed);

            if(breedId == Guid.Empty)
            {
                var newBreeds = new List<Breed>();
                if (command.Breed != null)
                {
                    var breed = new Breed(Name.Create(command.Breed).Value);
                    newBreeds.AddRange([breed]);
                    speciesExist.Value.AddBreeds(newBreeds.ToList());
                    await _speciesRepository.Save(speciesExist.Value, cancellationToken);
                    breedId = breed.Id;
                }
            }

            var speciesBreed = new SpeciesBreed(speciesId, breedId.Value);

            var description = Description.Create(command.Description).Value;
            var color = Color.Create(command.Color).Value;
            var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
            var address = Address.Create(
                command.Address.Region,
                command.Address.City,
                command.Address.Street,
                command.Address.House,
                command.Address.Floor,
                command.Address.Apartment).Value;
            var ownerPhoneNumber = PhoneNumber.Create(
                command.OwnerPhoneNumber.Value,
                command.OwnerPhoneNumber.IsMain).Value;
            var assistanceStatus = AssistanceStatus.Create(
                command.AssistanceStatus).Value;

            var assistanceDetails = new List<AssistanceDetail>();
            if (command.AssistanceDetail != null)
            {
                var detail = AssistanceDetail.Create(
                    command.AssistanceDetail.Name,
                    command.AssistanceDetail.Description,
                    command.AssistanceDetail.AccountNumber).Value;
                assistanceDetails.AddRange([detail]);
            }
            var petAssistanceDetails = new PetAssistanceDetails(
                assistanceDetails);

            var newPet = new Pet(
                petId,
                nicKName,
                speciesBreed,
                description,
                color,
                healthInfo,
                address,
                command.Weight,
                command.Height,
                ownerPhoneNumber,
                command.IsCastrated,
                command.IsVaccinated,
                command.DateOfBirth,
                assistanceStatus,
                petAssistanceDetails,
                DateOnly.FromDateTime(DateTime.Today));

            var petPhotos = command.PetPhotos.Select(p => PetPhoto.Create(
                p.FileName,
                p.IsMain).Value);

            foreach (var petPhoto in petPhotos)
                newPet.AddPetPhoto(petPhoto);

            volunteerResult.Value.AddPet(newPet);

            await _volunteersRepository.Save(
                volunteerResult.Value,
                cancellationToken);

            return (Guid)newPet.Id;
        }
    }
}
