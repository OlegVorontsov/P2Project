using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Pets.GetPetById;

public class GetPetByIdValidator :
    AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdValidator()
    {
        RuleFor(p => p.PetId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}