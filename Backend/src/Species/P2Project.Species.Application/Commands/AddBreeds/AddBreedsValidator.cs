using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Application.Commands.AddBreeds
{
    public class AddBreedsValidator :
        AbstractValidator<AddBreedsCommand>
    {
        public AddBreedsValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(s => s.Breeds)
                .MustBeValueObject(b => Name.Create(b.Name.Value));
        }
    }
}
