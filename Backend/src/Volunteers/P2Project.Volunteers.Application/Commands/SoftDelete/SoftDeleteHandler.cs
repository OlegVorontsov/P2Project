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

namespace P2Project.Volunteers.Application.Commands.SoftDelete
{
    public class SoftDeleteHandler :
        ICommandHandler<Guid, SoftDeleteCommand>
    {
        private readonly IValidator<SoftDeleteCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SoftDeleteHandler> _logger;
        public SoftDeleteHandler(
            IValidator<SoftDeleteCommand> validator,
            IVolunteersRepository volunteersRepository,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<SoftDeleteHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            SoftDeleteCommand command,
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

            volunteerResult.Value.SoftDelete();

            _volunteersRepository.Save(volunteerResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Volunteer with ID: {VolunteerId} was deleted",
                command.VolunteerId);

            return volunteerResult.Value.Id.Value;
        }
    }
}
