using P2Project.Core.Dtos.Pets;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Species.Application.Commands.Create
{
    public record CreateCommand(
        NameDto Name,
        IEnumerable<BreedDto> Breeds) : ICommand;
}
