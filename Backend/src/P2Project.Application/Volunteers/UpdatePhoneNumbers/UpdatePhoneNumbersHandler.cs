using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Shared;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public class UpdatePhoneNumbersHandler
    {
        private readonly IValidator<UpdatePhoneNumbersCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePhoneNumbersHandler> _logger;
        public UpdatePhoneNumbersHandler(
            IValidator<UpdatePhoneNumbersCommand> validator,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdatePhoneNumbersHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid,ErrorList>> Handle(
            UpdatePhoneNumbersCommand command,
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
                volunteerId, cancellationToken);
            if(volunteerResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.VolunteerId);
                return error.ToErrorList();
            }

            var newPhoneNumbers = new List<PhoneNumber>();

            var existingPhoneNumbers = volunteerResult.Value.PhoneNumbers;
            if (existingPhoneNumbers != null)
            {
                var oldPhonesToAdd = existingPhoneNumbers?
                                    .PhoneNumbers
                                    .Select(pn => PhoneNumber
                                        .Create(
                                        pn.Value,
                                        pn.IsMain).Value);
                if(oldPhonesToAdd != null)
                    newPhoneNumbers.AddRange(oldPhonesToAdd);
            }

            if (command.PhoneNumbers != null)
            {
                var phonesToAdd = command
                                    .PhoneNumbers
                                    .Select(pn => PhoneNumber
                                        .Create(
                                        pn.Value,
                                        pn.IsMain).Value);
                newPhoneNumbers.AddRange(phonesToAdd);
            }

            var volunteerPhones = new VolunteerPhoneNumbers(newPhoneNumbers);

            volunteerResult.Value.UpdatePhoneNumbers(volunteerPhones);

            var id = _volunteersRepository.Save(
                            volunteerResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated phone numbers",
                    id);

            return id;
        }
    }
}
