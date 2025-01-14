using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.ValueObjects;

namespace P2Project.Species.Application
{
    public interface ISpeciesRepository
    {
        Task<Guid> Add(
                    Domain.Species species,
                    CancellationToken cancellationToken = default);
        
        Guid Save(
            Domain.Species species);

        Guid Delete(
            Domain.Species species,
            CancellationToken cancellationToken = default);
        
        Task<Result<Domain.Species, Error>> GetById(
                    SpeciesId speciesId,
                    CancellationToken cancellationToken = default);
        
        Task<Result<Domain.Species, Error>> GetByName(
                    Name name,
                    CancellationToken cancellationToken = default);
    }
}