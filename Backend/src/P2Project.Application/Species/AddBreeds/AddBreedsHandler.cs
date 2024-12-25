using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;

namespace P2Project.Application.Species.AddBreeds
{
    public class AddBreedsHandler : ICommandHandler<Guid, AddBreedsCommand>
    {
        private readonly IValidator<AddBreedsCommand> _validator;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<AddBreedsHandler> _logger;
        public AddBreedsHandler(
            IValidator<AddBreedsCommand> validator,
            ISpeciesRepository speciesRepository,
            ILogger<AddBreedsHandler> logger)
        {
            _validator = validator;
            _speciesRepository = speciesRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            AddBreedsCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var speciesId = SpeciesId.Create(command.SpeciesId);

            var speciesResult = await _speciesRepository.GetById(
                speciesId, cancellationToken);
            if (speciesResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.SpeciesId);
                return error.ToErrorList();
            }

            var newBreeds = new List<Breed>();
            if (command.Breeds != null)
            {
                var breedsToAdd = command.Breeds
                                         .Select(b => Breed
                                             .Create(b.Name.Value).Value);
                newBreeds.AddRange(breedsToAdd);
            }

            speciesResult.Value?.AddBreeds(newBreeds);

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
