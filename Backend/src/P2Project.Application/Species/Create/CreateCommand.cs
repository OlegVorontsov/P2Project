using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Species.Create
{
    public record CreateCommand(
        NameDto Name,
        IEnumerable<BreedDto> Breeds);
}
