using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Errors;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;

namespace P2Project.Species.Application.Queries.GetAllBreedsPaginatedBySpeciesId;

public class GetAllBreedsPaginatedBySpeciesIdHandler :
    IQueryValidationHandler<PagedList<BreedReadDto>, GetAllBreedsPaginatedBySpeciesIdQuery>
{
    private readonly IValidator<GetAllBreedsPaginatedBySpeciesIdQuery> _validator;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetAllBreedsPaginatedBySpeciesIdHandler> _logger;

    public GetAllBreedsPaginatedBySpeciesIdHandler(
        IValidator<GetAllBreedsPaginatedBySpeciesIdQuery> validator,
        IReadDbContext readDbContext,
        ILogger<GetAllBreedsPaginatedBySpeciesIdHandler> logger)
    {
        _validator = validator;
        _readDbContext = readDbContext;
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
        
        var species = await _readDbContext.Species
            .AnyAsync(s => s.Id == query.SpeciesId, cancellationToken);
        if (species == false)
        {
            _logger.LogInformation("Requested species with id {id} was not found", query.SpeciesId);
            return Errors.General.NotFound(query.SpeciesId).ToErrorList();
        }
        
        var breedsQuery = _readDbContext.Breeds
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