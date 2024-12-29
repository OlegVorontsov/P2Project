using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Species.Commands.DeleteBreedById;

public class DeleteSpeciesByIdValidator : AbstractValidator<DeleteBreedByIdCommand>
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