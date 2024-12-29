﻿using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Common;
using P2Project.Domain.PetManagment.ValueObjects.Pets;
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
                .NotNull()
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
            
            RuleFor(ap => ap.SpeciesId)
                .NotNull()
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
            
            RuleFor(ap => ap.BreedId)
                .NotNull()
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(ap => ap.NickName).MustBeValueObject(NickName.Create);

            RuleFor(ap => ap.Description).MustBeValueObject(Description.Create);

            RuleFor(ap => ap.Color).MustBeValueObject(Color.Create);

            RuleFor(ap => ap.HealthInfo).MustBeValueObject(hi => HealthInfo.Create(
                hi.Weight,
                hi.Height,
                hi.IsCastrated,
                hi.IsVaccinated,
                hi.HealthDescription));

            RuleFor(ap => ap.Address).MustBeValueObject(a => Address.Create(
                a.Region,
                a.City,
                a.Street,
                a.House,
                a.Floor,
                a.Apartment));

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
