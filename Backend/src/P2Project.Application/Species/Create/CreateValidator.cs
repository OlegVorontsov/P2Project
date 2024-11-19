using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Species.Create
{
    public class CreateValidator :
        AbstractValidator<CreateRequest>
    {
        public CreateValidator()
        {
            RuleFor(c => c.Name).MustBeValueObject(n =>
                        Name.Create(n.Value));

            RuleFor(c => c.Breeds.Select(b => b.Name)).NotEmpty();
        }
    }
}
