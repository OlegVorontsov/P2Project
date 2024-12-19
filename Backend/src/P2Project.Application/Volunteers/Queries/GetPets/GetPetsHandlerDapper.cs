using System.Text;
using System.Text.Json;
using Dapper;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Volunteers.Queries.GetPets;

public class GetPetsHandlerDapper : IQueryHandler<PagedList<PetDto>, GetPetsQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetPetsHandlerDapper(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetPetsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _connectionFactory.CreateConnection();
            
        var parameters = new DynamicParameters();

        var totalCount = await connection
            .ExecuteScalarAsync<long>("select count(*) from pets");

        var sql = new StringBuilder(
            """
            select id, nick_name, position, photos from pets
            """
        );

        if (!string.IsNullOrWhiteSpace(query.NickName))
        {
            sql.Append(" WHERE nickname like @NickName");
            parameters.Add("@NickName", query.NickName);
        }
            
        sql.ApplySorting(parameters, query.SortBy, query.SortOrder);
        sql.ApplyPagination(parameters, query.Page, query.PageSize);
            
        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            sql.ToString(),
            (pet, jsonPhotos) =>
            {
                var photos = JsonSerializer.Deserialize<IEnumerable<PetPhotoDto>>(jsonPhotos) ?? new List<PetPhotoDto>();
                pet.Photos = photos;
                return pet;
            },
            splitOn: "photos",
            param: parameters);
            
        return new PagedList<PetDto>()
        {
            Items = pets.ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}