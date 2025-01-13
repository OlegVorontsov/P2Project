using FluentValidation;
using P2Project.Core.Errors;
using P2Project.Core.Validation;

namespace P2Project.Species.Application.Queries.GetAllBreedsPaginatedBySpeciesId;

public class GetAllBreedsPaginatedBySpeciesIdValidator :
    AbstractValidator<GetAllBreedsPaginatedBySpeciesIdQuery>
{
    public GetAllBreedsPaginatedBySpeciesIdValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));

        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}