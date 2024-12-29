using FluentValidation;

namespace P2Project.Application.Volunteers.Commands.Delete
{
    public class DeleteValidator :
        AbstractValidator<DeleteCommand>
    {
        public DeleteValidator()
        {
            RuleFor(d => d.VolunteerId).NotNull().NotEmpty();
        }
    }
}
