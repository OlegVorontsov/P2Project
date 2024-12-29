using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Interfaces.Agreements;

public interface ISpeciesAgreement
{
    Task<UnitResult<Error>> SpeciesAndBreedExists(
        Guid SpeciesId, Guid BreedId,
        CancellationToken cancellationToken = default);
}