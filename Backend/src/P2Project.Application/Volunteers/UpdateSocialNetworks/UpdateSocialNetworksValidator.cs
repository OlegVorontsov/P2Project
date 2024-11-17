﻿using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksValidator :
        AbstractValidator<UpdateSocialNetworksRequest>
    {
        public UpdateSocialNetworksValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(s => s.SocialNetworksDto.SocialNetworks)
                .MustBeValueObject(sn => SocialNetwork.Create(
                                   sn.Name,
                                   sn.Link));
        }
    }
}