using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler :
    IQueryValidationHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;

    public GetVolunteerByIdHandler(
        IVolunteersReadDbContext volunteersReadDbContext,
        ILogger<GetVolunteerByIdHandler> logger)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteerDto = await _volunteersReadDbContext.Volunteers
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