using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Species.Queries.IsAnyPetBySpecies;

public class IsAnyPetBySpeciesHandler : IQueryHandler<bool, IsAnyPetBySpeciesQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<IsAnyPetBySpeciesHandler> _logger;

    public IsAnyPetBySpeciesHandler(
        IReadDbContext readDbContext,
        ILogger<IsAnyPetBySpeciesHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<bool, ErrorList>> Handle(
        IsAnyPetBySpeciesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _readDbContext.Pets.AnyAsync(p =>
            p.SpeciesId == query.SpeciesId, cancellationToken);
        if (result)
            _logger.LogWarning("There're pets with species id {SpeciesId}", query.SpeciesId);
        return result;
    }
}