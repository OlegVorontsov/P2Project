using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Volunteers.Commands.AddPet
{
    public class AddPetValidator :
        AbstractValidator<AddPetCommand>
    {
        public AddPetValidator()
        {
            RuleFor(ap => ap.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(ap => ap.NickName).MustBeValueObject(NickName.Create);

            RuleFor(ap => ap.Species).MustBeValueObject(Name.Create);

            RuleFor(ap => ap.Breed).MustBeValueObject(Name.Create);

            RuleFor(ap => ap.Description).MustBeValueObject(Description.Create);

            RuleFor(ap => ap.Color).MustBeValueObject(Color.Create);

            RuleFor(ap => ap.HealthInfo).MustBeValueObject(HealthInfo.Create);

            RuleFor(ap => ap.Address).MustBeValueObject(a => Address.Create(
                a.Region,
                a.City,
                a.Street,
                a.House,
                a.Floor,
                a.Apartment));

            RuleFor(ap => ap.Weight).InclusiveBetween(
                Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT)
                .WithError(Errors.General.ValueIsInvalid("Weight"));

            RuleFor(ap => ap.Height).InclusiveBetween(
                Constants.MIN_WEIGHT_HEIGHT, Constants.MAX_WEIGHT_HEIGHT)
                .WithError(Errors.General.ValueIsInvalid("Height"));

            RuleFor(ap => ap.OwnerPhoneNumber).MustBeValueObject(pn =>
                                            PhoneNumber.Create(
                                                pn.Value,
                                                pn.IsMain));

            RuleFor(ap => ap.AssistanceStatus).MustBeValueObject(
                AssistanceStatus.Create);

            RuleFor(ap => ap.AssistanceDetail).MustBeValueObject(ad =>
                                            AssistanceDetail.Create(
                                                ad.Name,
                                                ad.Description,
                                                ad.AccountNumber));
        }
    }
}
