using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.FileProvider;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.HardDelete;

public class HardDeleteHandler :
    ICommandHandler<Guid, HardDeleteCommand>
{
    private readonly IValidator<HardDeleteCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HardDeleteHandler> _logger;

    public HardDeleteHandler(
        IValidator<HardDeleteCommand> validator,
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        ILogger<HardDeleteHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        HardDeleteCommand command,
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
        
        var deletingResult = _volunteersRepository.Delete(volunteerResult.Value);

        if (deletingResult.IsFailure)
        {
            _logger.LogError("Error occured while deleting volunteer's (id = {vId})",
                volunteerId);
            return deletingResult.Error.ToErrorList();
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        return volunteerResult.Value.Id.Value;
    }
}