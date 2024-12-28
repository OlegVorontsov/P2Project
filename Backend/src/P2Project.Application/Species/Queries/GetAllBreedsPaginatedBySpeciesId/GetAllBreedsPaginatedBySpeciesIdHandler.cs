using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Models;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.GetAllBreedsPaginatedBySpeciesId;

public class GetAllBreedsPaginatedBySpeciesIdHandler :
    IQueryHandler<PagedList<BreedReadDto>, GetAllBreedsPaginatedBySpeciesIdQuery>
{
    private readonly IValidator<GetAllBreedsPaginatedBySpeciesIdQuery> _validator;
    private readonly ISpeciesReadDbContext _speciesReadDbContext;
    private readonly ILogger<GetAllBreedsPaginatedBySpeciesIdHandler> _logger;

    public GetAllBreedsPaginatedBySpeciesIdHandler(
        IValidator<GetAllBreedsPaginatedBySpeciesIdQuery> validator,
        ISpeciesReadDbContext speciesReadDbContext,
        ILogger<GetAllBreedsPaginatedBySpeciesIdHandler> logger)
    {
        _validator = validator;
        _speciesReadDbContext = speciesReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<BreedReadDto>, ErrorList>> Handle(
        GetAllBreedsPaginatedBySpeciesIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var species = await _speciesReadDbContext.Species
            .AnyAsync(s => s.Id == query.SpeciesId, cancellationToken);
        if (species == false)
        {
            _logger.LogInformation("Requested species with id {id} was not found", query.SpeciesId);
            return Errors.General.NotFound(query.SpeciesId).ToErrorList();
        }
        
        var breedsQuery = _speciesReadDbContext.Breeds
            .Where(b => b.SpeciesId == query.SpeciesId);
        
        var keySelector = SortByProperty(query.SortBy);
        
        breedsQuery = query.SortDirection?.ToLower() == "desc"
            ? breedsQuery.OrderByDescending(keySelector)
            : breedsQuery.OrderBy(keySelector);
        
        breedsQuery.WhereIf(
            string.IsNullOrWhiteSpace(query.Name),
            s => s.Name.Contains(query.Name!));

        return await breedsQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
    
    private static Expression<Func<BreedReadDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return breed => breed.Id;

        Expression<Func<BreedReadDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "name" => b => b.Name,
            _ => b => b.Id
        };

        return keySelector;
    }
}