using FluentValidation;

namespace P2Project.Volunteers.Application.Commands.SoftDelete
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
