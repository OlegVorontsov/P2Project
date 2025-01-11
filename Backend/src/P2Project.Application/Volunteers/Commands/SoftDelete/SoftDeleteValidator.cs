using FluentValidation;

namespace P2Project.Application.Volunteers.Commands.SoftDelete
{
    public class SoftDeleteValidator :
        AbstractValidator<SoftDeleteCommand>
    {
        public SoftDeleteValidator()
        {
            RuleFor(d => d.VolunteerId).NotNull().NotEmpty();
        }
    }
}
