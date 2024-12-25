using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdQueryHandler :
    IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetVolunteerByIdQueryHandler> _logger;

    public GetVolunteerByIdQueryHandler(
        IReadDbContext readDbContext,
        ILogger<GetVolunteerByIdQueryHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteerDto = await _readDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);

        if (volunteerDto is null)
        {
            _logger.LogWarning("Failed to query volunteer with id {id}", query.Id);
            return Errors.General.NotFound(query.Id).ToErrorList();
        }

        _logger.LogInformation("Volunteer with id {id} queried", query.Id);

        return volunteerDto;
    }
}