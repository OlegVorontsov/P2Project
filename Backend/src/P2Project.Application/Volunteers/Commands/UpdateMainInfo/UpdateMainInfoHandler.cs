using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IValidator<UpdateMainInfoCommand> validator,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
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

            var fullName = FullName.Create(
                                    command.FullName.FirstName,
                                    command.FullName.SecondName,
                                    command.FullName.LastName).Value;

            var volunteerInfo = VolunteerInfo.Create(
                command.VolunteerInfo.Age,
                command.VolunteerInfo.Grade).Value;

            var gender = Enum.Parse<Gender>(command.Gender);

            var description = Description.Create(
                command.Description).Value;

            volunteerResult.Value.UpdateMainInfo(
                fullName,
                volunteerInfo,
                gender,
                description);

            var id = _volunteersRepository.Save(
                                        volunteerResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated main info to " +
                    "full name: {SecondName} {FirstName} {LastName} " +
                    "age: {Age} grade: {Grade} gender: {gender} " +
                    "description: {Value}",
                    id,
                    fullName.SecondName,
                    fullName.FirstName,
                    fullName.LastName,
                    volunteerInfo.Age,
                    volunteerInfo.Grade,
                    gender,
                    description.Value);

            return id;
        }
    }
}
