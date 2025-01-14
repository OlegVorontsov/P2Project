using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Agreements;

namespace P2Project.Species.Application.Commands.DeleteSpeciesById;

public class DeleteSpeciesByIdHandler :
    ICommandHandler<Guid, DeleteSpeciesByIdCommand>
{
    private readonly IValidator<DeleteSpeciesByIdCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IPetsAgreement _petsAgreement;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesByIdHandler> _logger;
    
    public DeleteSpeciesByIdHandler(
        IValidator<DeleteSpeciesByIdCommand> validator,
        ISpeciesRepository speciesRepository,
        IPetsAgreement petsAgreement,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesByIdHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _petsAgreement = petsAgreement;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesId = SpeciesId.Create(command.Id);
        
        var speciesToDelete = await _speciesRepository.GetById(
            speciesId, cancellationToken);
        if (speciesToDelete.IsFailure)
            return Errors.General.NotFound(speciesId.Value).ToErrorList();
        
        var isAnyPet = await _petsAgreement.IsAnyPetBySpeciesId(
            speciesId.Value, cancellationToken);
        if (isAnyPet)
            return Errors.General
                .DeleteConflict(speciesId.Value, nameof(Species)).ToErrorList();

        _speciesRepository.Delete(
            speciesToDelete.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Species with name {name} and id {id}",
            speciesToDelete.Value.Name,
            speciesToDelete.Value.Id);

        return speciesToDelete.Value.Id.Value;
    }
}