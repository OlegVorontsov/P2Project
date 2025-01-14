using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using P2Project.Core.Dtos.Pets;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.Core.Models;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetPets
{
    public class GetPetsHandler :
        IQueryValidationHandler<PagedList<PetDto>, GetPetsQuery>
    {
        private readonly IValidator<GetPetsQuery> _validator;
        private readonly IReadDbContext _readDbContext;

        public GetPetsHandler(
            IReadDbContext readDbContext, IValidator<GetPetsQuery> validator)
        {
            _readDbContext = readDbContext;
            _validator = validator;
        }

        public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
            GetPetsQuery query,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(
                query, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var petsQuery = ApplyFilters(_readDbContext.Pets, query);
            
            var keySelector = SortByProperty(query.SortBy);
            
            petsQuery = query.SortOrder?.ToLower() == "desc"
                ? petsQuery.OrderByDescending(keySelector)
                : petsQuery.OrderBy(keySelector);
            
            var result = await petsQuery.ToPagedListOrError(
                    query.Page,
                    query.PageSize,
                    cancellationToken);

            if (result.IsFailure)
            {
                var error = result.Error;
                return error.ToErrorList();
            }

            return result.Value;
        }

        private static Expression<Func<PetDto, object>> SortByProperty(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                return volunteer => volunteer.Id;

            Expression<Func<PetDto, object>> keySelector = sortBy?.ToLower() switch
            {
                "nickname" => p => p.NickName,
                "color" => p => p.Color,
                "city" => p => p.City,
                "weight" => p => p.Weight,
                "height" => p => p.Height,
                _ => p => p.Id
            };

            return keySelector;
        }
        
        private static IQueryable<PetDto> ApplyFilters(
            IQueryable<PetDto> dbQuery, GetPetsQuery query)
        {
            return dbQuery
                .WhereIf(query.VolunteerId.GetValueOrDefault(Guid.Empty) != Guid.Empty, p => p.VolunteerId == query.VolunteerId)
                .WhereIf(query.SpeciesId.GetValueOrDefault(Guid.Empty) != Guid.Empty, p => p.SpeciesId == query.SpeciesId)
                .WhereIf(query.BreedId.GetValueOrDefault(Guid.Empty) != Guid.Empty, p => p.BreedId == query.BreedId)
                .WhereIf(!string.IsNullOrWhiteSpace(query.NickName), p => p.NickName.Contains(query.NickName!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.Color), p => p.Color.Contains(query.Color!))
                .WhereIf(!string.IsNullOrWhiteSpace(query.City), p => p.City.Contains(query.City!))
                .WhereIf(query.WeightFrom.HasValue, p => p.Weight >= query.WeightFrom!)
                .WhereIf(query.WeightTo.HasValue, p => p.Weight <= query.WeightTo!)
                .WhereIf(query.HeightFrom.HasValue, p => p.Height >= query.HeightFrom!)
                .WhereIf(query.HeightTo.HasValue, p => p.Height <= query.HeightTo!);
        }
    }
}