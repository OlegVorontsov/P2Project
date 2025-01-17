using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.DataBase;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetPets;

public class GetPetsHandlerDapper :
    IQueryValidationHandler<PagedList<PetDto>, GetPetsQuery>
{
    private readonly IValidator<GetPetsQuery> _validator;
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetPetsHandlerDapper(
        IValidator<GetPetsQuery> validator,
        ISqlConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
        GetPetsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
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