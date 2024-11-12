using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;

        public CreateVolunteerHandler(
            IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }
        public async Task<Result<Guid, Error>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var fullName = FullName.Create(
                                   command.FullName.FirstName,
                                   command.FullName.SecondName,
                                   command.FullName.LastName).Value;

            var volunteerByFullName = await _volunteersRepository.GetByFullName(fullName);
            if (volunteerByFullName.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var gender = Enum.Parse<Gender>(command.Gender);

            var email = Email.Create(command.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var description = Description.Create(command.Description).Value;

            var registeredDate = DateTime.Now;

            var phoneNumbers = new List<PhoneNumber>();
            if (command.PhoneNumbers != null)
            {
                foreach (var number in command.PhoneNumbers)
                {
                    var phoneNumberResult = PhoneNumber.Create(number.Value, number.IsMain);
                    if (phoneNumberResult.IsFailure)
                        return phoneNumberResult.Error;
                    phoneNumbers.Add(phoneNumberResult.Value);
                }
            }
            var volunteerPhoneNumbers = new VolunteerPhoneNumbers(phoneNumbers);

            var socialNetworks = new List<SocialNetwork>();
            if (command.SocialNetworks != null)
            {
                foreach (var socialNetwork in command.SocialNetworks)
                {
                    var socialNetworkResult = SocialNetwork.Create(
                                                            socialNetwork.Name,
                                                            socialNetwork.Link);
                    if (socialNetworkResult.IsFailure)
                        return socialNetworkResult.Error;
                    socialNetworks.Add(socialNetworkResult.Value);
                }
            }
            var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

            var assistanceDetails = new List<AssistanceDetail>();
            if (command.AssistanceDetails != null)
            {
                foreach (var assistanceDetail in command.AssistanceDetails)
                {
                    var assistanceDetailResult = AssistanceDetail.Create(
                                                 assistanceDetail.Name,
                                                 assistanceDetail.Description,
                                                 assistanceDetail.AccountNumber);
                    if (assistanceDetailResult.IsFailure)
                        return assistanceDetailResult.Error;
                    assistanceDetails.Add(assistanceDetailResult.Value);
                }
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

            return (Guid)volunteer.Id;
        }
    }
}
