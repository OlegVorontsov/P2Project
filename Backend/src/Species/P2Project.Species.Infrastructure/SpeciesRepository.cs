using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Application;
using P2Project.Species.Domain.ValueObjects;
using P2Project.Species.Infrastructure.DbContexts;

namespace P2Project.Species.Infrastructure
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly SpeciesWriteDbContext _dbContext;
        public SpeciesRepository(SpeciesWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Add(
                                Domain.Species species,
                                CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return species.Id;
        }

        public Guid Save(Domain.Species species)
        {
            _dbContext.Species.Attach(species);
            return species.Id.Value;
        }

        public Guid Delete(
            Domain.Species species, CancellationToken cancellationToken = default)
        {
            _dbContext.Species.Remove(species);
            return species.Id.Value;
        }

        public async Task<Result<Domain.Species, Error>> GetById(
                                 SpeciesId speciesId,
                                 CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => 
                        s.Id == speciesId, cancellationToken);
            if (species is null)
                return Errors.General.NotFound(speciesId);
            return species;
        }

        public async Task<Result<Domain.Species, Error>> GetByName(
                                Name name,
                                CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species
                .FirstOrDefaultAsync(s => 
                        s.Name == name, cancellationToken);
            if (species is null)
                return Errors.General.NotFound();
            return species;
        }
    }
}
