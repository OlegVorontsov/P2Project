using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.API.Controllers.Species.Requests
{
    public record AddBreedsRequest(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto);
}
