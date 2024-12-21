using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.API.Controllers.Species.Requests
{
    public record CreateSpeciesRequest(
        NameDto Name,
        IEnumerable<BreedDto> Breeds);
}
