using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Controllers.Pets.Requests
{
    public record GetPetsRequest(
        string? NickName,
        int Page,
        int PageSize)
    {
        public GetPetsQuery ToQuery() =>
            new (
                NickName,
                Page,
                PageSize);
    }
}
