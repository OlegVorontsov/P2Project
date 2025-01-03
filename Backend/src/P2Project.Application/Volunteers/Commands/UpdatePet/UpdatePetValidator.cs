using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Commands.UpdatePet;

public class UpdatePetValidator :
    AbstractValidator<UpdatePetCommand>
{
    public UpdatePetValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.SpeciesId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
            
        RuleFor(p => p.BreedId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(p => p.NickName).MustBeValueObject(NickName.Create);
        
        RuleFor(p => p.Description).MustBeValueObject(Description.Create);

        RuleFor(p => p.Color).MustBeValueObject(Color.Create);

        RuleFor(p => p.HealthInfo).MustBeValueObject(hi => HealthInfo.Create(
            hi.Weight,
            hi.Height,
            hi.IsCastrated,
            hi.IsVaccinated,
            hi.HealthDescription));

        RuleFor(p => p.Address).MustBeValueObject(a => Address.Create(
            a.Region,
            a.City,
            a.Street,
            a.House,
            a.Floor,
            a.Apartment));

        RuleFor(p => p.OwnerPhoneNumber).MustBeValueObject(pn =>
            PhoneNumber.Create(
                pn.Value,
                pn.IsMain));
        
        RuleFor(p => p.BirthDate).NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("BirthDate"));

        RuleFor(p => p.AssistanceStatus).MustBeValueObject(
            AssistanceStatus.Create);

        RuleFor(p => p.AssistanceDetails).MustBeValueObject(ad =>
            AssistanceDetail.Create(
                ad.Name,
                ad.Description,
                ad.AccountNumber));
    }
}