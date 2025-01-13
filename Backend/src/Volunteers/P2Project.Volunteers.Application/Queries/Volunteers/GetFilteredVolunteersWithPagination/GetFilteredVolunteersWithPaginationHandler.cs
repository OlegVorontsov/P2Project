using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Core.Errors;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationHandler :
    IQueryValidationHandler<PagedList<VolunteerDto>, 
        GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;
    private readonly IReadDbContext _readDbContext;
    
    public GetFilteredVolunteersWithPaginationHandler(
        IReadDbContext readDbContext,
        IValidator<GetFilteredVolunteersWithPaginationQuery> validator)
    {
        _readDbContext = readDbContext;
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
        
        var volunteersQuery = _readDbContext.Volunteers;

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