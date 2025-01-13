using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Core.Errors;
using P2Project.Core.Extensions;
using P2Project.Core.IDs;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Volunteers.Agreements;

namespace P2Project.Species.Application.Commands.DeleteBreedById;

public class DeleteBreedByIdHandler :
    ICommandHandler<Guid, DeleteBreedByIdCommand>
{
    private readonly IValidator<DeleteBreedByIdCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IPetsAgreement _petsAgreement;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBreedByIdHandler> _logger;

    public DeleteBreedByIdHandler(
        IValidator<DeleteBreedByIdCommand> validator,
        ISpeciesRepository speciesRepository,
        IPetsAgreement petsAgreement,
        IUnitOfWork unitOfWork,
        ILogger<DeleteBreedByIdHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _petsAgreement = petsAgreement;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator
            .ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var isAnyPet = await _petsAgreement.IsAnyPetByBreedId(
            command.BreedId, cancellationToken);
        if (isAnyPet)
            return Errors.General.DeleteConflict(command.BreedId).ToErrorList();

        var speciesResult = await _speciesRepository.GetById(
            SpeciesId.Create(command.SpeciesId), cancellationToken);
        if (speciesResult.IsFailure)
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();

        var breedToDelete = speciesResult.Value.Breeds
            .FirstOrDefault(b => b.Id == command.BreedId);
        if (breedToDelete is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var deletingResult = speciesResult.Value.DeleteBreed(breedToDelete);
        if (deletingResult.IsFailure)
            return deletingResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Deleted Breed with name {name} and id {id}",
            breedToDelete.Name,
            breedToDelete.Id);

        return breedToDelete.Id.Value;
    }
}