using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Application.Volunteers.UpdateMainInfo;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public class UpdatePhoneNumbersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateMainInfoHandler> _logger;
        public UpdatePhoneNumbersHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }
        public async Task<Result<Guid,Error>> Handle(
            UpdatePhoneNumbersCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if(volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

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

            if (command.PhoneNumbersDto != null)
            {
                var phonesToAdd = command
                                    .PhoneNumbersDto
                                    .PhoneNumbers
                                    .Select(pn => PhoneNumber
                                        .Create(
                                        pn.Value,
                                        pn.IsMain).Value);
                newPhoneNumbers.AddRange(phonesToAdd);
            }

            var volunteerPhones = new VolunteerPhoneNumbers(newPhoneNumbers);

            volunteerResult.Value.UpdatePhoneNumbers(volunteerPhones);

            var id = await _volunteersRepository.Save(
                            volunteerResult.Value,
                            cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated phone numbers",
                    id);

            return id;
        }
    }
}
