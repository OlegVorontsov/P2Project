using CSharpFunctionalExtensions;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }
        public async Task<Result<Guid, Error>> Handle(
            CreateCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var fullNameResult = FullName.Create(command.fullName.FirstName,
                                                 command.fullName.SecondName,
                                                 command.fullName.LastName);
            if (fullNameResult.IsFailure)
                return fullNameResult.Error;

            var volunteerByFullName = await _volunteersRepository.GetByFullName(fullNameResult.Value);
            if (volunteerByFullName.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var age = command.age;
            if (age < Constants.MIN_AGE || age > Constants.MAX_AGE)
                return Errors.General.ValueIsInvalid("age");

            var genderResult = Enum.Parse<Gender>(command.gender);
            if (genderResult != Gender.Male && genderResult != Gender.Female)
                return Errors.General.ValueIsInvalid("gender");

            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(emailResult.Value);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var descriptionResult = Description.Create(command.Description);
            if (descriptionResult.IsFailure)
                return descriptionResult.Error;

            var registeredDate = DateTime.Now;

            var phoneNumbers = new List<PhoneNumber>();
            if (command.phoneNumbers != null)
            {
                foreach (var number in command.phoneNumbers)
                {
                    var phoneNumberResult = PhoneNumber.Create(number.Value, number.IsMain);
                    if (phoneNumberResult.IsFailure)
                        return phoneNumberResult.Error;
                    phoneNumbers.Add(phoneNumberResult.Value);
                }
            }
            var volunteerPhoneNumbers = new VolunteerPhoneNumbers(phoneNumbers);

            var socialNetworks = new List<SocialNetwork>();
            if (command.socialNetworks != null)
            {
                foreach (var socialNetwork in command.socialNetworks)
                {
                    var socialNetworkResult = SocialNetwork.Create(socialNetwork.Name,
                                                                   socialNetwork.Link);
                    if (socialNetworkResult.IsFailure)
                        return socialNetworkResult.Error;
                    socialNetworks.Add(socialNetworkResult.Value);
                }
            }
            var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

            var assistanceDetails = new List<AssistanceDetail>();
            if (command.assistanceDetails != null)
            {
                foreach (var assistanceDetail in command.assistanceDetails)
                {
                    var assistanceDetailResult = AssistanceDetail.Create(assistanceDetail.Name,
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
                            fullNameResult.Value,
                            age,
                            genderResult,
                            emailResult.Value,
                            descriptionResult.Value,
                            registeredDate,
                            volunteerPhoneNumbers,
                            volunteerSocialNetworks,
                            volunteerAssistanceDetails);

            await _volunteersRepository.Add(volunteer, cancellationToken);

            return (Guid)volunteer.Id;
        }
    }
}
