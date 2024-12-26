using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Species.Commands.Create
{
    public class CreateValidator :
        AbstractValidator<CreateCommand>
    {
        public CreateValidator()
        {
            RuleFor(c => c.Name).MustBeValueObject(n =>
                        Name.Create(n.Value));

            RuleFor(c => c.Breeds.Select(b => b.Name)).NotEmpty();
        }
    }
}
