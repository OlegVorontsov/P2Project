using FluentValidation;

namespace P2Project.Species.Application.Commands.DeleteSpeciesById;

public class DeleteSpeciesByIdValidator :
    AbstractValidator<DeleteSpeciesByIdCommand>
{
    public DeleteSpeciesByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}