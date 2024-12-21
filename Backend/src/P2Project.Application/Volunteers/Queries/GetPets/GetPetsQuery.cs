using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public record GetPetsQuery(
        Guid? VolunteerId,
        Guid? SpeciesId,
        Guid? BreedId,  
        string? NickName,
        string? Color,
        string? City,
        int? WeightFrom,
        int? WeightTo,
        int? HeightFrom,
        int? HeightTo,
        string? SortBy,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery;
}
