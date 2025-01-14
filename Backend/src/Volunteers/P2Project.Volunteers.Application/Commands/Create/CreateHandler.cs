using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

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
            IUnitOfWork unitOfWork,
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

            var fullName = FullName.Create(
                                   command.FullName.FirstName,
                                   command.FullName.SecondName,
                                   command.FullName.LastName).Value;
            var volunteerByFullName = await _volunteersRepository.GetByFullName(
                                                                  fullName,
                                                                  cancellationToken);
            if (volunteerByFullName.IsSuccess)
            {
                var error = Errors.VolunteerError.AlreadyExist();
                return error.ToErrorList();
            }
            
            var volunteerInfo = VolunteerInfo.Create(
                command.VolunteerInfo.Age,
                command.VolunteerInfo.Grade).Value;

            var gender = Enum.Parse<Gender>(command.Gender);

            var email = Email.Create(command.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(
                                                               email,
                                                               cancellationToken);
            if (volunteerByEmail.IsSuccess)
            {
                var error = Errors.VolunteerError.AlreadyExist();
                return error.ToErrorList();
            }

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

            var socialNetworks = new List<SocialNetwork>();
            if (command.SocialNetworks != null)
            {
                var networks = command.SocialNetworks.Select(sn =>
                                                      SocialNetwork.Create(
                                                         sn.Name,
                                                         sn.Link).Value);
                socialNetworks.AddRange(networks);
            }
            var volunteerSocialNetworks = socialNetworks;

            var assistanceDetails = new List<AssistanceDetail>();
            if (command.AssistanceDetails != null)
            {
                var details = command.AssistanceDetails.Select(ad =>
                                                        AssistanceDetail.Create(
                                                            ad.Name,
                                                            ad.Description,
                                                            ad.AccountNumber).Value);
                assistanceDetails.AddRange(details);
            }
            var volunteerAssistanceDetails = assistanceDetails;

            var volunteer = new Volunteer(
                            volunteerId,
                            fullName,
                            volunteerInfo,
                            gender,
                            email,
                            description,
                            volunteerPhoneNumbers,
                            volunteerSocialNetworks,
                            volunteerAssistanceDetails);

            await _volunteersRepository.Add(volunteer, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Volunteer created with ID: {id}",
                volunteerId.Value);

            return (Guid)volunteer.Id;
        }
    }
}
