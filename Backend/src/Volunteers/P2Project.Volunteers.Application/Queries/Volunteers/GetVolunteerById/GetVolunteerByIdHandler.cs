using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetVolunteerById;

public class GetVolunteerByIdHandler :
    IQueryValidationHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;

    public GetVolunteerByIdHandler(
        IVolunteersReadDbContext readDbContext,
        ILogger<GetVolunteerByIdHandler> logger)
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