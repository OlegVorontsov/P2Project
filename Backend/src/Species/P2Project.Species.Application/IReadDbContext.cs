using P2Project.Core.Dtos.Pets;

namespace P2Project.Species.Application;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedReadDto> Breeds { get; }
}