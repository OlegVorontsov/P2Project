using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Core.Dtos.VolunteerRequests;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Domain.Enums;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmitted;

public class GetAllSubmittedHandler :
    IQueryValidationHandler<PagedList<VolunteerRequestDto>, 
        GetAllSubmittedQuery>
{
    private readonly IValidator<GetAllSubmittedQuery> _validator;
    private readonly IVolunteerRequestsReadDbContext _readDbContext;

    public GetAllSubmittedHandler(
        IValidator<GetAllSubmittedQuery> validator,
        IVolunteerRequestsReadDbContext readDbContext)
    {
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> Handle(
        GetAllSubmittedQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var requestsQuery = _readDbContext.VolunteerRequests.WhereIf(true,
            x => x.Status == RequestStatus.Submitted.ToString());
        
        return await requestsQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}