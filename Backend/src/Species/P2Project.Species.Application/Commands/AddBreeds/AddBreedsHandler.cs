using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.Entities;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Application.Commands.AddBreeds
{
    public class AddBreedsHandler : ICommandHandler<Guid, AddBreedsCommand>
    {
        private readonly IValidator<AddBreedsCommand> _validator;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddBreedsHandler> _logger;
        public AddBreedsHandler(
            IValidator<AddBreedsCommand> validator,
            ISpeciesRepository speciesRepository,
            ILogger<AddBreedsHandler> logger,
            [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _speciesRepository = speciesRepository;
            _unitOfWork = unitOfWork;
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
                return Errors.General.NotFound(command.SpeciesId).ToErrorList();

            var newBreeds = new List<Breed>();
            if (command.Breeds != null)
            {
                var breedsToAdd = command.Breeds
                                         .Select(b => new Breed(
                                             BreedId.New(),
                                             Name.Create(b.Name.Value).Value));
                newBreeds.AddRange(breedsToAdd);
            }

            var addBreedResult = speciesResult.Value?.AddBreeds(newBreeds);
            if(addBreedResult.Value.IsFailure)
                return addBreedResult.Value.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For species with ID: {id} was updated breeds",
                    speciesResult.Value.Id.Value);

            return speciesResult.Value.Id.Value;
        }
    }
}
