using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails
{
    public class UpdateAssistanceDetailsHandler : ICommandHandler<Guid, UpdateAssistanceDetailsCommand>
    {
        private readonly IValidator<UpdateAssistanceDetailsCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAssistanceDetailsHandler> _logger;
        public UpdateAssistanceDetailsHandler(
            IValidator<UpdateAssistanceDetailsCommand> validator,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateAssistanceDetailsHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateAssistanceDetailsCommand command,
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
            if (volunteerResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.VolunteerId);
                return error.ToErrorList();
            }

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

            if (command.AssistanceDetails != null)
            {
                var assistanceDetailsToAdd = command
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
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated assistance details",
                    id);

            return id;
        }
    }
}
