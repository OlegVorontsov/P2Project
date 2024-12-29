using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment.ValueObjects;

namespace P2Project.Application.Interfaces.DataBase
{
    public interface ISpeciesRepository
    {
        Task<Guid> Add(
                    Domain.SpeciesManagment.Species species,
                    CancellationToken cancellationToken = default);
        
        Guid Save(
            Domain.SpeciesManagment.Species species);

        Guid Delete(
            Domain.SpeciesManagment.Species species,
            CancellationToken cancellationToken = default);
        
        Task<Result<Domain.SpeciesManagment.Species, Error>> GetById(
                    SpeciesId speciesId,
                    CancellationToken cancellationToken = default);
        
        Task<Result<Domain.SpeciesManagment.Species, Error>> GetByName(
                    Name name,
                    CancellationToken cancellationToken = default);
    }
}