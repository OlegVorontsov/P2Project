using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.HardDeletePet;

public class HardDeletePetHandler :
    ICommandHandler<Guid, HardDeletePetCommand>
{
    private readonly IValidator<HardDeletePetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HardDeletePetHandler> _logger;

    public HardDeletePetHandler(
        IValidator<HardDeletePetCommand> validator,
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<HardDeletePetHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        HardDeletePetCommand command,
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
        var deletingResult = volunteerResult.Value.HardDeletePet(petId);

        if (deletingResult.IsFailure)
        {
            _logger.LogError("Error occured while deleting volunteer's (id = {vId}) pet (id = {pId})",
                volunteerId,
                petId);
            return deletingResult.Error.ToErrorList();
        }

        var filePathsToDelete = deletingResult.Value;

        foreach (var filePath in filePathsToDelete)
        {
            var fileDeletingResult = await _fileProvider.DeleteFileByFileMetadata(
                new FileMetadata(Constants.BUCKET_NAME_PHOTOS, filePath),
                cancellationToken);

            if (fileDeletingResult.IsFailure)
                _logger.LogError("Error occured while deleting file with name {name} from storage",
                    filePath);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return volunteerResult.Value.Id.Value;
    }
}