using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;

public class CreateVolunteerRequestValidator :
    AbstractValidator<CreateVolunteerRequestCommand>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(vr => vr.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(vr => vr.FullName).MustBeValueObject(fn =>
            FullName.Create(fn.FirstName, fn.SecondName, fn.LastName));
        
        RuleFor(vr => vr.VolunteerInfo).MustBeValueObject(vi =>
            VolunteerInfo.Create(vi.Age, vi.Grade));
    }
}