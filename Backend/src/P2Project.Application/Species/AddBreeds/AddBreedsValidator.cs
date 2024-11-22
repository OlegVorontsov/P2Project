using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared;
using P2Project.Domain.SpeciesManagment.Entities;

namespace P2Project.Application.Species.AddBreeds
{
    public class AddBreedsValidator :
        AbstractValidator<AddBreedsRequest>
    {
        public AddBreedsValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(s => s.AddBreedsDto.Breeds)
                .MustBeValueObject(b => Breed.Create(b.Name.Value));
        }
    }
}
