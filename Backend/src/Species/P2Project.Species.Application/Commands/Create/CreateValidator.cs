using FluentValidation;
using P2Project.Core.Validation;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Application.Commands.Create
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
