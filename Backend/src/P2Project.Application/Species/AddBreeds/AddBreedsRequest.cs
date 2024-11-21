using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Validation;
using P2Project.Application.Volunteers;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using System.Collections.Generic;

namespace P2Project.Application.Species.AddBreeds
{
    public record AddBreedsRequest(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto);

    public record AddBreedsDto(
        IEnumerable<BreedDto> Breeds);

    public record AddBreedsCommand(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto);

    public class AddBreedsValidator :
        AbstractValidator<AddBreedsRequest>
    {
        public AddBreedsValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(s => s.AddBreedsDto.Breeds)
                .MustBeValueObject(b => Breed.Create(b.Name.Value));
        }
    }

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
            var speciesId = SpeciesId.CreateSpeciesId(
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
