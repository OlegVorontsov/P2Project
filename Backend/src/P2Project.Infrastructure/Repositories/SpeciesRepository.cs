using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.Shared;
using P2Project.Domain.SpeciesManagment;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using P2Project.Application.Species;
using P2Project.Domain.Shared.Errors;
using P2Project.Infrastructure.DbContexts;

namespace P2Project.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly WriteDbContext _dbContext;
        public SpeciesRepository(WriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Add(
                                Species species,
                                CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return species.Id;
        }

        public Guid Save(Species species)
        {
            _dbContext.Species.Attach(species);
            return species.Id.Value;
        }

        public Guid Delete(
            Species species, CancellationToken cancellationToken = default)
        {
            _dbContext.Species.Remove(species);
            return species.Id.Value;
        }

        public async Task<Result<Species, Error>> GetById(
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

        public async Task<Result<Species, Error>> GetByName(
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
