using FluentValidation;
using P2Project.Core.Validation;
using P2Project.SharedKernel.Errors;

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