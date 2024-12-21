using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Species;
using P2Project.Application.Species.Create;
using P2Project.Domain.PetManagment.Entities;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Volunteers.Commands.AddPet
{
    public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private const string BUCKET_NAME = "photos";

        private readonly IValidator<AddPetCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddPetHandler> _petLogger;
        private readonly ILogger<CreateHandler> _speciesLogger;

        public AddPetHandler(
            IValidator<AddPetCommand> validator,
            IVolunteersRepository volunteersRepository,
            ISpeciesRepository speciesRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddPetHandler> petLogger,
            ILogger<CreateHandler> speciesLogger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
            _petLogger = petLogger;
            _speciesLogger = speciesLogger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                          command,
                          cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(
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

            if (speciesIfExist.IsFailure)
            {
                var newBreeds = new List<Breed>();
                if (command.Breed != null)
                {
                    var breed = new Breed(Name.Create(command.Breed).Value);
                    newBreeds.AddRange([breed]);
                }

                var species = new Domain.SpeciesManagment.Species(
                    SpeciesId.New(), speciesName, newBreeds);

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

            if (breedId == Guid.Empty)
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
            var healthInfo = HealthInfo.Create(
                command.HealthInfo.Weight,
                command.HealthInfo.Height,
                command.HealthInfo.IsCastrated,
                command.HealthInfo.IsVaccinated,
                command.HealthInfo.HealthDescription).Value;
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
            var petAssistanceDetails = assistanceDetails;

            var newPet = new Pet(
                petId,
                nicKName,
                speciesBreed,
                description,
                color,
                healthInfo,
                address,
                ownerPhoneNumber,
                command.BirthDate,
                assistanceStatus,
                DateOnly.FromDateTime(DateTime.Today),
                petAssistanceDetails);

            volunteerResult.Value.AddPet(newPet);

            _volunteersRepository.Save(volunteerResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            _petLogger.LogInformation("Pet added with id: {PetId}.",
                newPet.Id.Value);

            return newPet.Id.Value;
        }
    }
}
