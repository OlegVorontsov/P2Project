using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Application.Interfaces.DbContexts.Species;

public interface ISpeciesReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedReadDto> Breeds { get; }
}