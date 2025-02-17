using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Core.Dtos.VolunteerRequests;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Domain.Enums;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmittedRequests;

public class GetAllSubmittedRequestsHandler :
    IQueryValidationHandler<PagedList<VolunteerRequestDto>, 
        GetAllSubmittedRequestsQuery>
{
    private readonly IValidator<GetAllSubmittedRequestsQuery> _validator;
    private readonly IVolunteerRequestsReadDbContext _readDbContext;

    public GetAllSubmittedRequestsHandler(
        IValidator<GetAllSubmittedRequestsQuery> validator,
        IVolunteerRequestsReadDbContext readDbContext)
    {
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> Handle(
        GetAllSubmittedRequestsQuery query,
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