using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.SoftDeletePet;

public class SoftDeletePetHandler :
    ICommandHandler<Guid, SoftDeletePetCommand>
{
    private readonly IValidator<SoftDeletePetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetCommand> _logger;

    public SoftDeletePetHandler(
        IValidator<SoftDeletePetCommand> validator,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetCommand> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerId = VolunteerId.Create(
            command.VolunteerId);

        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        volunteerResult.Value.SoftDeletePet(petId);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Softdeleted pet (id = {petId}) belonging to volunteer (id = {vid})",
            petId,
            volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}