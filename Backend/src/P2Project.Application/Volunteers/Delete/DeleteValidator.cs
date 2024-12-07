using FluentValidation;

namespace P2Project.Application.Volunteers.Delete
{
    public class DeleteValidator :
        AbstractValidator<DeleteCommand>
    {
        public DeleteValidator()
        {
            RuleFor(d => d.VolunteerId).NotEmpty();
        }
    }
}
