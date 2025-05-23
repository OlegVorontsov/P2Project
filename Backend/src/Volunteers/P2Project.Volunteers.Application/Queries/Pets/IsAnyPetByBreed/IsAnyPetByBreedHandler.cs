using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Interfaces.Queries;
using P2Project.Volunteers.Application.Interfaces;

namespace P2Project.Volunteers.Application.Queries.Pets.IsAnyPetByBreed;

public class IsAnyPetByBreedHandler :
    IQueryHandler<bool, IsAnyPetByBreedQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;
    private readonly ILogger<IsAnyPetByBreedHandler> _logger;

    public IsAnyPetByBreedHandler(
        IVolunteersReadDbContext readDbContext,
        ILogger<IsAnyPetByBreedHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
        IsAnyPetByBreedQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _readDbContext.Pets.AnyAsync(p =>
            p.BreedId == query.BreedId, cancellationToken);
        if (result)
            _logger.LogWarning("There're pets with breed id {BreedId}", query.BreedId);
        return result;
    }
}