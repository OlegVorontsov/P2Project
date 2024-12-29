using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Shared;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.Delete
{
    public class DeleteHandler : ICommandHandler<Guid, DeleteCommand>
    {
        private readonly IValidator<DeleteCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteHandler> _logger;
        public DeleteHandler(
            IValidator<DeleteCommand> validator,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteCommand command,
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
