using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public record GetPetsQuery(
        string? NickName,
        int? PositionFrom,
        int? PositionTo,
        string? SortBy,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery;
}
