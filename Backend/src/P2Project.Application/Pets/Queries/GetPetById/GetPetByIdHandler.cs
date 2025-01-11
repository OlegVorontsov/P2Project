using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Pets.Queries.GetPetById;

public class GetPetByIdHandler :
    IQueryValidationHandler<PetDto, GetPetByIdQuery>
{
    private readonly IValidator<GetPetByIdQuery> _validator;
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ILogger<GetPetByIdHandler> _logger;

    public GetPetByIdHandler(
        IValidator<GetPetByIdQuery> validator,
        IVolunteersReadDbContext volunteersReadDbContext,
        ILogger<GetPetByIdHandler> logger)
    {
        _validator = validator;
        _volunteersReadDbContext = volunteersReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var petDto = await _volunteersReadDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);

        if (petDto is null)
        {
            _logger.LogWarning("Failed to query pet with id {id}", query.PetId);
            return Errors.General.NotFound(query.PetId).ToErrorList();
        }

        _logger.LogInformation("Pet with id {id} queried", query.PetId);

        return petDto;
    }
}