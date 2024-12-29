using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Species.Commands.AddBreeds;

namespace P2Project.API.Controllers.Species.Requests;

public record AddBreedsRequest(
    IEnumerable<BreedDto> Breeds)
{
    public AddBreedsCommand ToCommand(Guid speciesId) =>
        new(speciesId, Breeds);
}
