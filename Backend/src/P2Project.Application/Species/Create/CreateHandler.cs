using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.Repositories;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Create
{
    public class CreateHandler : ICommandHandler<Guid, CreateCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<CreateHandler> _logger;

        public CreateHandler(
            ISpeciesRepository speciesRepository,
            ILogger<CreateHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
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
                var breeds = command.Breeds.Select(bDto => new Breed(Name.Create(bDto.Name.Value).Value));
                newBreeds.AddRange(breeds);
            }

            var species = new Domain.SpeciesManagment.Species(
                speciesId, name, newBreeds);

            await _speciesRepository.Add(species, cancellationToken);

            _logger.LogInformation(
                "Species created with ID: {id}",
                speciesId.Value);

            return (Guid)species.Id;
        }
    }
}
