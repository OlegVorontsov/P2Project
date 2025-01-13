using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Errors;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;

namespace P2Project.Species.Application.Queries.GetAllSpeciesFilteredPaginated;

public class GetAllSpeciesFilteredPaginatedQueryHandler :
    IQueryValidationHandler<PagedList<SpeciesDto>, GetAllSpeciesFilteredPaginatedQuery>
{
    private readonly IValidator<GetAllSpeciesFilteredPaginatedQuery> _validator;
    private readonly IReadDbContext _readDbContext;
    
    public GetAllSpeciesFilteredPaginatedQueryHandler(
        IValidator<GetAllSpeciesFilteredPaginatedQuery> validator,
        IReadDbContext readDbContext)
    {
        _validator = validator;
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetAllSpeciesFilteredPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesQuery = _readDbContext.Species;
        
        var keySelector = SortByProperty(query.SortBy);
        
        speciesQuery = query.SortDirection?.ToLower() == "desc"
            ? speciesQuery.OrderByDescending(keySelector)
            : speciesQuery.OrderBy(keySelector);
        
        speciesQuery.WhereIf(
            string.IsNullOrWhiteSpace(query.Name),
            s => s.Name.Contains(query.Name!));

        return await speciesQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
    
    private static Expression<Func<SpeciesDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return species => species.Id;

        Expression<Func<SpeciesDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "name" => s => s.Name,
            _ => s => s.Id
        };

        return keySelector;
    }
}