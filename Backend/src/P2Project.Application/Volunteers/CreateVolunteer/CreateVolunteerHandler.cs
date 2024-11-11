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
            CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            // create newVolunteer and validation
            var volunteerId = VolunteerId.NewVolunteerId();

            // fullName
            var fullNameResult = FullName.Create(request.fullName.FirstName,
                                                 request.fullName.SecondName,
                                                 request.fullName.LastName);
            if (fullNameResult.IsFailure)
                return fullNameResult.Error;

            var volunteerByFullName = await _volunteersRepository.GetByFullName(fullNameResult.Value);
            if (volunteerByFullName.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            // age
            var age = request.age;
            if (age < Constants.MIN_AGE || age > Constants.MAX_AGE)
                return Errors.General.ValueIsInvalid("age");

            // gender
            var genderResult = Enum.Parse<Gender>(request.gender);
            if (genderResult != Gender.Male && genderResult != Gender.Female)
                return Errors.General.ValueIsInvalid("gender");

            // email
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(emailResult.Value);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            // description
            var descriptionResult = Description.Create(request.Description);
            if (descriptionResult.IsFailure)
                return descriptionResult.Error;

            // RegisteredDate
            var registeredDate = DateTime.Now;

            // phoneNumbers
            var phoneNumbers = new List<PhoneNumber>();
            if (request.phoneNumbers != null)
            {
                foreach (var number in request.phoneNumbers)
                {
                    var phoneNumberResult = PhoneNumber.Create(number.Value, number.IsMain);
                    if (phoneNumberResult.IsFailure)
                        return phoneNumberResult.Error;
                    phoneNumbers.Add(phoneNumberResult.Value);
                }
            }
            var volunteerPhoneNumbers = new VolunteerPhoneNumbers(phoneNumbers);

            // socialNetworks
            var socialNetworks = new List<SocialNetwork>();
            if (request.socialNetworks != null)
            {
                foreach (var socialNetwork in request.socialNetworks)
                {
                    var socialNetworkResult = SocialNetwork.Create(socialNetwork.Name,
                                                                   socialNetwork.Link);
                    if (socialNetworkResult.IsFailure)
                        return socialNetworkResult.Error;
                    socialNetworks.Add(socialNetworkResult.Value);
                }
            }
            var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

            // assistanceDetails
            var assistanceDetails = new List<AssistanceDetail>();
            if (request.assistanceDetails != null)
            {
                foreach (var assistanceDetail in request.assistanceDetails)
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

            var volunteer = Volunteer.Create(volunteerId,
                                                     fullNameResult.Value,
                                                     age,
                                                     genderResult,
                                                     emailResult.Value,
                                                     descriptionResult.Value,
                                                     registeredDate,
                                                     volunteerPhoneNumbers,
                                                     volunteerSocialNetworks,
                                                     volunteerAssistanceDetails);

            await _volunteersRepository.Add(volunteer.Value, cancellationToken);

            return (Guid)volunteer.Value.Id;
        }
    }
}
