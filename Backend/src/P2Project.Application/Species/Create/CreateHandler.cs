using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Create
{
    public class CreateHandler : ICommandHandler<Guid, CreateCommand>
    {
        private readonly IValidator<CreateCommand> _validator;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<CreateHandler> _logger;

        public CreateHandler(
            IValidator<CreateCommand> validator,
            ISpeciesRepository speciesRepository,
            ILogger<CreateHandler> logger)
        {
            _validator = validator;
            _speciesRepository = speciesRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var speciesId = SpeciesId.New();

            var name = Name.Create(
                command.Name.Value).Value;
            
            var speciesByName = await _speciesRepository.GetByName(
                name, cancellationToken);
            if (speciesByName.IsSuccess)
            {
                var error = Errors.Species.AlreadyExist();
                return error.ToErrorList();
            }

            var newBreeds = new List<Breed>();
            if (command.Breeds != null)
            {
                var breeds = command.Breeds
                    .Select(bDto => new Breed(Name.Create(bDto.Name.Value).Value));
                newBreeds.AddRange(breeds);
            }

            var newSpecies = new Domain.SpeciesManagment.Species(
                speciesId, name, newBreeds);

            await _speciesRepository.Add(newSpecies, cancellationToken);

            _logger.LogInformation(
                "Species created with ID: {id}",
                newSpecies.Id.Value);

            return newSpecies.Id.Value;
        }
    }
}
