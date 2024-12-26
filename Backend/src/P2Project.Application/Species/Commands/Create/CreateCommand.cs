using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Application.Species.Commands.Create
{
    public record CreateCommand(
        NameDto Name,
        IEnumerable<BreedDto> Breeds) : ICommand;
}
