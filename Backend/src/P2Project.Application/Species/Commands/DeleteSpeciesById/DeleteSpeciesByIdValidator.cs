using FluentValidation;

namespace P2Project.Application.Species.Commands.DeleteSpeciesById;

public class DeleteSpeciesByIdValidator : AbstractValidator<DeleteSpeciesByIdCommand>
{
    public DeleteSpeciesByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}