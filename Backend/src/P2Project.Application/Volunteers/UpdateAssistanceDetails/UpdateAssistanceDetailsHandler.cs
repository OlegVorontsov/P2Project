using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.UpdateAssistanceDetails
{
    public class UpdateAssistanceDetailsHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateAssistanceDetailsHandler> _logger;
        public UpdateAssistanceDetailsHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdateAssistanceDetailsHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateAssistanceDetailsCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            var newAssistanceDetails = new List<AssistanceDetail>();

            var existingAssistanceDetails = volunteerResult.Value.AssistanceDetails;
            if (existingAssistanceDetails != null)
            {
                var oldAssistanceDetails = existingAssistanceDetails?
                                    .AssistanceDetails
                                    .Select(ad => AssistanceDetail
                                        .Create(
                                            ad.Name,
                                            ad.Description,
                                            ad.AccountNumber).Value);
                if (oldAssistanceDetails != null)
                    newAssistanceDetails.AddRange(oldAssistanceDetails);
            }

            if (command.AssistanceDetailsDto != null)
            {
                var assistanceDetailsToAdd = command
                                    .AssistanceDetailsDto
                                    .AssistanceDetails
                                    .Select(ad => AssistanceDetail
                                        .Create(
                                            ad.Name,
                                            ad.Description,
                                            ad.AccountNumber).Value);
                newAssistanceDetails.AddRange(assistanceDetailsToAdd);
            }

            var volunteerAssistanceDetails = new VolunteerAssistanceDetails(
                                                newAssistanceDetails);

            volunteerResult.Value.UpdateAssistanceDetails(volunteerAssistanceDetails);

            var id = _volunteersRepository.Save(
                            volunteerResult.Value);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated assistance details",
                    id);

            return id;
        }
    }
}
