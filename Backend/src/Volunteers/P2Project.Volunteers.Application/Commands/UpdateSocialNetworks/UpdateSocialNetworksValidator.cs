using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Volunteers.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksValidator :
        AbstractValidator<UpdateSocialNetworksCommand>
    {
        public UpdateSocialNetworksValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(s => s.SocialNetworks)
                .MustBeValueObject(sn => SocialNetwork.Create(
                                   sn.Name,
                                   sn.Link));
        }
    }
}
