using P2Project.Core.Dtos.Pets;

namespace P2Project.Species.Application;

public interface ISpeciesReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedReadDto> Breeds { get; }
}