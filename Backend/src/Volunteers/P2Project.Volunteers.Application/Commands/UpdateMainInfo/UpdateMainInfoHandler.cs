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
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Domain;

namespace P2Project.Volunteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler :
        ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IValidator<UpdateMainInfoCommand> validator,
            IVolunteersRepository volunteersRepository,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateMainInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                                      command,
                                      cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(
                                          command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                                        volunteerId,
                                        cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteerInfo = VolunteerInfo.Create(
                command.VolunteerInfo.Age,
                command.VolunteerInfo.Grade).Value;

            var gender = Enum.Parse<Gender>(command.Gender);

            var description = Description.Create(
                command.Description).Value;

            volunteerResult.Value.UpdateMainInfo(
                volunteerInfo,
                gender,
                description);

            var id = _volunteersRepository.Save(
                                        volunteerResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated main info to " +
                    "age: {Age} grade: {Grade} gender: {gender} " +
                    "description: {Value}",
                    id,
                    volunteerInfo.Age,
                    volunteerInfo.Grade,
                    gender,
                    description.Value);

            return id;
        }
    }
}
