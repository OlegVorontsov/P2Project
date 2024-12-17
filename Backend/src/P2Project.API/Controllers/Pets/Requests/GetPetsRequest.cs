using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Controllers.Pets.Requests
{
    public record GetPetsRequest(
        string? NickName,
        int? PositionFrom,
        int? PositionTo,
        int Page,
        int PageSize)
    {
        public GetPetsQuery ToQuery() =>
            new (
                NickName,
                PositionFrom,
                PositionTo,
                Page,
                PageSize);
    }
}
