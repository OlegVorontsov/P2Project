using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;

namespace P2Project.Species.Application.Commands.DeleteBreedById;

public class DeleteSpeciesByIdValidator :
    AbstractValidator<DeleteBreedByIdCommand>
{
    public DeleteSpeciesByIdValidator()
    {
        RuleFor(x => x.SpeciesId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid(nameof(SpeciesId)));
        
        RuleFor(x => x.BreedId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("BreedId"));
    }
}