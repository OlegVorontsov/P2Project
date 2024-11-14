using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public class CreateHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<CreateHandler> _logger;

        public CreateHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<CreateHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            CreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateCommand(
                              request.FullName,
                              request.Age,
                              request.Gender,
                              request.Email,
                              request?.Description,
                              request.PhoneNumbers,
                              request?.SocialNetworks,
                              request?.AssistanceDetails);

            var volunteerId = VolunteerId.NewVolunteerId();

            var fullName = FullName.Create(
                                   command.FullName.FirstName,
                                   command.FullName.SecondName,
                                   command.FullName.LastName).Value;
            var volunteerByFullName = await _volunteersRepository.GetByFullName(
                                                                  fullName,
                                                                  cancellationToken);
            if (volunteerByFullName.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var gender = Enum.Parse<Gender>(command.Gender);

            var email = Email.Create(command.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(
                                                               email,
                                                               cancellationToken);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var description = Description.Create(command.Description).Value;

            var registeredDate = DateTime.Now;

            var phoneNumbers = new List<PhoneNumber>();
            if (command.PhoneNumbers != null)
            {
                var phones = command.PhoneNumbers.Select(pn =>
                                                  PhoneNumber.Create(
                                                      pn.Value,
                                                      pn.IsMain).Value);
                phoneNumbers.AddRange(phones);
            }
            var volunteerPhoneNumbers = new VolunteerPhoneNumbers(phoneNumbers);

            var socialNetworks = new List<SocialNetwork>();
            if (command.SocialNetworks != null)
            {
                var networks = command.SocialNetworks.Select(sn =>
                                                      SocialNetwork.Create(
                                                         sn.Name,
                                                         sn.Link).Value);
                socialNetworks.AddRange(networks);
            }
            var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

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
            var volunteerAssistanceDetails = new VolunteerAssistanceDetails(assistanceDetails);

            var volunteer = new Volunteer(
                            volunteerId,
                            fullName,
                            command.Age,
                            gender,
                            email,
                            description,
                            registeredDate,
                            volunteerPhoneNumbers,
                            volunteerSocialNetworks,
                            volunteerAssistanceDetails);

            await _volunteersRepository.Add(volunteer, cancellationToken);

            _logger.LogInformation(
                "Volunteer created with ID: {id}",
                volunteerId.Value);

            return (Guid)volunteer.Id;
        }
    }
}
