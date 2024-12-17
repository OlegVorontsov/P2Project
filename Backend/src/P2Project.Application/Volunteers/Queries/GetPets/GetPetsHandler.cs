using System.Text.Json;
using Dapper;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public class GetPetsHandler : IQueryHandler<PagedList<PetDto>, GetPetsQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsHandler(
            IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsQuery query,
            CancellationToken cancellationToken)
        {
            var petsQuery = _readDbContext.Pets;

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.NickName),
                p => p.NickName.Contains(query.NickName!));
            
            petsQuery = petsQuery.WhereIf(
                query.PositionTo != null,
                p => p.Position <= query.PositionTo!.Value);
            
            petsQuery = petsQuery.WhereIf(
                query.PositionFrom != null,
                p => p.Position >= query.PositionFrom!.Value);

            return await petsQuery
                .OrderBy(p => p.Position)
                .ToPagedList(
                query.Page,
                query.PageSize,
                cancellationToken);
        }
    }
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

            var sql = """
                      select id, nick_name, position, photos from pets
                      order by position desc
                      limit @pageSize offset @offset
                      """;

            parameters.Add("@pageSize", query.PageSize);
            parameters.Add("@offset", (query.Page - 1) * query.PageSize);
            
            var pets = await connection.QueryAsync<PetDto, string, PetDto>(
                sql,
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
}
