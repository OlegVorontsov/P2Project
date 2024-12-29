using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Models;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.GetAllSpeciesFilteredPaginated;

public class GetAllSpeciesFilteredPaginatedQueryHandler :
    IQueryValidationHandler<PagedList<SpeciesDto>, GetAllSpeciesFilteredPaginatedQuery>
{
    private readonly IValidator<GetAllSpeciesFilteredPaginatedQuery> _validator;
    private readonly ISpeciesReadDbContext _speciesReadDbContext;
    
    public GetAllSpeciesFilteredPaginatedQueryHandler(
        IValidator<GetAllSpeciesFilteredPaginatedQuery> validator,
        ISpeciesReadDbContext speciesReadDbContext)
    {
        _validator = validator;
        _speciesReadDbContext = speciesReadDbContext;
    }
    
    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetAllSpeciesFilteredPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesQuery = _speciesReadDbContext.Species;
        
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