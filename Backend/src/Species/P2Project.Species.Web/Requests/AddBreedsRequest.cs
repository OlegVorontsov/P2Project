using P2Project.Core.Dtos.Pets;
using P2Project.Species.Application.Commands.AddBreeds;

namespace P2Project.Species.Web.Requests;

public record AddBreedsRequest(
    IEnumerable<BreedDto> Breeds)
{
    public AddBreedsCommand ToCommand(Guid speciesId) =>
        new(speciesId, Breeds);
}
