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
using P2Project.Volunteers.Domain;

namespace P2Project.Volunteers.Application.Commands.Create
{
    public class CreateHandler : ICommandHandler<Guid, CreateCommand>
    {
        private readonly IValidator<CreateCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateHandler> _logger;

        public CreateHandler(
            IValidator<CreateCommand> validator,
            IVolunteersRepository volunteersRepository,
            [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork,
            ILogger<CreateHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                                      command,
                                      cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.New();
            
            var volunteerInfo = VolunteerInfo.Create(
                command.VolunteerInfo.Age,
                command.VolunteerInfo.Grade).Value;

            var gender = Enum.Parse<Gender>(command.Gender);

            var description = Description.Create(command.Description).Value;

            var phoneNumbers = new List<PhoneNumber>();
            if (command.PhoneNumbers != null)
            {
                var phones = command.PhoneNumbers.Select(pn =>
                                                  PhoneNumber.Create(
                                                      pn.Value,
                                                      pn.IsMain).Value);
                phoneNumbers.AddRange(phones);
            }
            var volunteerPhoneNumbers = phoneNumbers;

            var volunteer = new Volunteer(
                            volunteerId,
                            volunteerInfo,
                            gender,
                            description,
                            volunteerPhoneNumbers);

            await _volunteersRepository.Add(volunteer, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Volunteer created with ID: {id}",
                volunteerId.Value);

            return (Guid)volunteer.Id;
        }
    }
}
