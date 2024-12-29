using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Species.Commands.AddBreeds
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
