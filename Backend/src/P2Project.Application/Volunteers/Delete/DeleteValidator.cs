using FluentValidation;

namespace P2Project.Application.Volunteers.Delete
{
    public class DeleteValidator :
        AbstractValidator<DeleteRequest>
    {
        public DeleteValidator()
        {
            RuleFor(d => d.VolunteerId).NotEmpty();
        }
    }
}
