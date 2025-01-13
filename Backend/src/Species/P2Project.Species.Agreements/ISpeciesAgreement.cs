using CSharpFunctionalExtensions;
using P2Project.Core.Errors;

namespace P2Project.Species.Agreements;

public interface ISpeciesAgreement
{
    Task<UnitResult<Error>> SpeciesAndBreedExists(
        Guid SpeciesId, Guid BreedId,
        CancellationToken cancellationToken = default);
}