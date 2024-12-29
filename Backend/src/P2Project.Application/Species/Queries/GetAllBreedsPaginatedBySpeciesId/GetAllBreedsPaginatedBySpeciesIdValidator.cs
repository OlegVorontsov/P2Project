using FluentValidation;
using P2Project.Application.Validation;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.GetAllBreedsPaginatedBySpeciesId;

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