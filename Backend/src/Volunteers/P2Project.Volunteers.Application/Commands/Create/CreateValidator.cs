﻿using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Volunteers.Application.Commands.Create
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

            RuleFor(c => c.VolunteerInfo).MustBeValueObject(vi =>
                                    VolunteerInfo.Create(
                                        vi.Age,
                                        vi.Grade));

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