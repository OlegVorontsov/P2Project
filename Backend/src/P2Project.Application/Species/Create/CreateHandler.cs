using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.Shared;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.SpeciesManagment.Entities;
using System.Linq;

namespace P2Project.Application.Species.Create
{
    public class CreateHandler
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
        public async Task<Result<Guid, Error>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.NewSpeciesId();

            var name = Name.Create(
                command.Name.Value).Value;
            var speciesByName = await _speciesRepository.GetByName(
                name, cancellationToken);
            if (speciesByName.IsSuccess)
                return Errors.Species.AlreadyExist();

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
