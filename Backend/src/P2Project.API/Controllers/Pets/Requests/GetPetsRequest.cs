using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Controllers.Pets.Requests
{
    public record GetPetsRequest(
        string? NickName,
        int? PositionFrom,
        int? PositionTo,
        string? SortBy,
        string? SortOrder,
        int Page,
        int PageSize)
    {
        public GetPetsQuery ToQuery() =>
            new (
                NickName,
                PositionFrom,
                PositionTo,
                SortBy,
                SortOrder,
                Page,
                PageSize);
    }
}
