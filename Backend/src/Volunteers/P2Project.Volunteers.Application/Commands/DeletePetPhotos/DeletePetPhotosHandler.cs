using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Files;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Volunteers.Application.Commands.DeletePetPhotos;

public class DeletePetPhotosHandler :
    ICommandHandler<DeletePetPhotosCommand>
{
    private readonly IValidator<DeletePetPhotosCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersReadDbContext _readDbContext;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetPhotosHandler> _logger;

    public DeletePetPhotosHandler(
        IValidator<DeletePetPhotosCommand> validator,
        IVolunteersRepository volunteersRepository,
        IVolunteersReadDbContext readDbContext,
        IFileProvider fileProvider,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<DeletePetPhotosHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petExists = await _readDbContext.Pets
            .AnyAsync(p => p.Id == (Guid)petId, cancellationToken);
        if (petExists == false)
            return Errors.General.NotFound(petId).ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _volunteersRepository
            .GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var deleteResult = volunteerResult.Value.DeletePetPhotos(petId);
        if (deleteResult.IsFailure)
            return deleteResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);

        foreach (var filePath in deleteResult.Value)
        {
            var fileDeletingResult = await _fileProvider.DeleteFileByFileMetadata(
                new FileMetadata(Constants.BUCKET_NAME_PHOTOS, filePath),
                cancellationToken);
            
            if (fileDeletingResult.IsFailure)
                _logger.LogError("Error occured while deleting file with name {name} from storage",
                    filePath);
        }
        
        _logger.LogInformation(
            "Successfully deleted all pet photos of pet with id {petId}",
            petId);

        return Result.Success<ErrorList>();
    }
}