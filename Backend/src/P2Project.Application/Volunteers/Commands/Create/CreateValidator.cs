using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.Create
{
    public class CreateValidator :
                 AbstractValidator<CreateCommand>
    {
        public CreateValidator()
        {
            RuleFor(c => c.FullName).MustBeValueObject(fn =>
                                    FullName.Create(
                                        fn.FirstName,
                                        fn.SecondName,
                                        fn.LastName));

            RuleFor(c => c.Age).InclusiveBetween(
                 Constants.MIN_AGE, Constants.MAX_AGE)
                .WithError(Errors.General.ValueIsInvalid("Age"));

            RuleFor(c => c.Gender).IsEnumName(typeof(Gender))
                .WithError(Errors.General.ValueIsInvalid("Gender"));

            RuleFor(c => c.Email).MustBeValueObject(Email.Create);

            RuleFor(c => c.Description).MustBeValueObject(Description.Create);

            RuleForEach(c => c.PhoneNumbers).MustBeValueObject(pn =>
                                            PhoneNumber.Create(
                                                pn.Value,
                                                pn.IsMain));

            RuleForEach(c => c.SocialNetworks).MustBeValueObject(sn =>
                                            SocialNetwork.Create(
                                                sn.Name,
                                                sn.Link));

            RuleForEach(c => c.AssistanceDetails).MustBeValueObject(ad =>
                                            AssistanceDetail.Create(
                                                ad.Name,
                                                ad.Description,
                                                ad.AccountNumber));
        }
    }
}