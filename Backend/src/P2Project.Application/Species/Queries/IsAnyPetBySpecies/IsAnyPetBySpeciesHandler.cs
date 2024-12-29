using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.IsAnyPetBySpecies;

public class IsAnyPetBySpeciesHandler : IQueryHandler<bool, IsAnyPetBySpeciesQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ILogger<IsAnyPetBySpeciesHandler> _logger;

    public IsAnyPetBySpeciesHandler(
        IVolunteersReadDbContext volunteersReadDbContext,
        ILogger<IsAnyPetBySpeciesHandler> logger)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
        IsAnyPetBySpeciesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _volunteersReadDbContext.Pets.AnyAsync(p =>
            p.SpeciesId == query.SpeciesId, cancellationToken);
        if (result)
            _logger.LogWarning("There're pets with species id {SpeciesId}", query.SpeciesId);
        return result;
    }
}