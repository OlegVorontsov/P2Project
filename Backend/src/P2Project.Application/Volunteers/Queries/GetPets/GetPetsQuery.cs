using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public record GetPetsQuery(
        Guid? VolunteerId,
        Guid? SpeciesId,
        Guid? BreedId,  
        string? NickName,
        string? Color,
        string? Country,
        string? City,
        int? WeightFrom,
        int? WeightTo,
        int? HeightFrom,
        int? HeightTo,
        int? PositionFrom,
        int? PositionTo,
        string? SortBy,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery;
}
