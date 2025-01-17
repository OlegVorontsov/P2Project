using P2Project.Core.Dtos.Pets;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Species.Application.Commands.AddBreeds
{
    public record AddBreedsCommand(
        Guid SpeciesId,
        IEnumerable<BreedDto> Breeds) : ICommand;
}
