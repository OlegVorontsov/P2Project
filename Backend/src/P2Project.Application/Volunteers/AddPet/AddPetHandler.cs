using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Application.Species;
using P2Project.Application.Species.Create;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using IFileProvider = P2Project.Application.FileProvider.IFileProvider;

namespace P2Project.Application.Volunteers.CreatePet
{
    public class AddPetHandler
    {
        private const string BUCKET_NAME = "photos";
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<AddPetHandler> _petLogger;
        private readonly ILogger<CreateHandler> _speciesLogger;

        public AddPetHandler(
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            IFileProvider fileProvider,
            ILogger<AddPetHandler> petLogger,
            ILogger<CreateHandler> speciesLogger)
        {
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _fileProvider = fileProvider;
            _petLogger = petLogger;
            _speciesLogger = speciesLogger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.New();
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
            {
                var error = Errors.General.ValueIsInvalid(command.Breed);
                return error.ToErrorList();
            }

            if(breedId == Guid.Empty)
            {
                var newBreeds = new List<Breed>();
                if (command.Breed != null)
                {
                    var breed = new Breed(Name.Create(command.Breed).Value);
                    newBreeds.AddRange([breed]);
                    speciesExist.Value.AddBreeds(newBreeds.ToList());
                    await _speciesRepository.Save(
                        speciesExist.Value, cancellationToken);
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

            volunteerResult.Value.AddPet(newPet);

            await _volunteersRepository.Save(
                volunteerResult.Value,
                cancellationToken);

            _petLogger.LogInformation("Pet added with id: {PetId}.",
                newPet.Id.Value);

            return (Guid)newPet.Id;
        }
    }
}
