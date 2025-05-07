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
using P2Project.Volunteers.Application.Interfaces;

namespace P2Project.Volunteers.Application.Commands.HardDelete;

public class HardDeleteHandler :
    ICommandHandler<Guid, HardDeleteCommand>
{
    private readonly IValidator<HardDeleteCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HardDeleteHandler> _logger;

    public HardDeleteHandler(
        IValidator<HardDeleteCommand> validator,
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
        ILogger<HardDeleteHandler> logger)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
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