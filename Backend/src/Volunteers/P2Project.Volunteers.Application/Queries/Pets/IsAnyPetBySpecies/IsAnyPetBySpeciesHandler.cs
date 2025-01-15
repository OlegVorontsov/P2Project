using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Pets.IsAnyPetBySpecies;

public class IsAnyPetBySpeciesHandler :
    IQueryHandler<bool, IsAnyPetBySpeciesQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;
    private readonly ILogger<IsAnyPetBySpeciesHandler> _logger;

    public IsAnyPetBySpeciesHandler(
        IVolunteersReadDbContext readDbContext,
        ILogger<IsAnyPetBySpeciesHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
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