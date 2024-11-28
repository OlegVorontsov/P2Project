using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;

namespace P2Project.Application.Species.AddBreeds
{
    public class AddBreedsHandler
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<AddBreedsHandler> _logger;
        public AddBreedsHandler(
            ISpeciesRepository speciesRepository,
            ILogger<AddBreedsHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            AddBreedsCommand command,
            CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.Create(
                command.SpeciesId);

            var speciesResult = await _speciesRepository.GetById(
                speciesId, cancellationToken);
            if (speciesResult.IsFailure)
                return Errors.General.NotFound(command.SpeciesId);

            var newBreeds = new List<Breed>();

            if (command.AddBreedsDto != null)
            {
                var breedsToAdd = command
                                    .AddBreedsDto
                                    .Breeds
                                    .Select(b => Breed
                                        .Create(
                                        b.Name.Value).Value);
                newBreeds.AddRange(breedsToAdd);
            }

            var addedBreeds = speciesResult.Value?.AddBreeds(
                newBreeds.ToList());

            var id = await _speciesRepository.Save(
                            speciesResult.Value,
                            cancellationToken);

            _logger.LogInformation(
                    "For species with ID: {id} was updated breeds",
                    id);

            return id;
        }
    }
}
