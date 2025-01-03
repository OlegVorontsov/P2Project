using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Volunteers;
using P2Project.Application.Shared.Models;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationHandler :
    IQueryValidationHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    
    public GetFilteredVolunteersWithPaginationHandler(
        IVolunteersReadDbContext volunteersReadDbContext,
        IValidator<GetFilteredVolunteersWithPaginationQuery> validator)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteersQuery = _volunteersReadDbContext.Volunteers;

        var keySelector = SortByProperty(query.SortBy);

        volunteersQuery = query.SortDirection?.ToLower() == "desc"
            ? volunteersQuery.OrderByDescending(keySelector)
            : volunteersQuery.OrderBy(keySelector);

        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            v => v.FirstName.Contains(query.Name!));

        return await volunteersQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
    
    private static Expression<Func<VolunteerDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return volunteer => volunteer.Id;

        Expression<Func<VolunteerDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "firstname" => v => v.FirstName,
            "secondname" => v => v.SecondName,
            "lastname" => v => v.LastName,
            _ => v => v.Id
        };

        return keySelector;
    }
}