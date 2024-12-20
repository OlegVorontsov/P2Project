﻿using System.Linq.Expressions;
using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public class GetPetsHandler : IQueryHandler<PagedList<PetDto>, GetPetsQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsHandler(
            IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsQuery query,
            CancellationToken cancellationToken)
        {
            var petsQuery = _readDbContext.Pets;
            
            Expression<Func<PetDto, object>> keySelector = 
                query.SortBy?.ToLower() switch
                {
                    "nickname" => pet => pet.NickName,
                    "position" => pet => pet.Position,
                    _ => pet => pet.Id
                };
            
            petsQuery = query.SortOrder?.ToLower() == "desc"
                ? petsQuery.OrderByDescending(keySelector)
                : petsQuery.OrderBy(keySelector);

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.NickName),
                p => p.NickName.Contains(query.NickName!));
            
            petsQuery = petsQuery.WhereIf(
                query.PositionTo != null,
                p => p.Position <= query.PositionTo!.Value);
            
            petsQuery = petsQuery.WhereIf(
                query.PositionFrom != null,
                p => p.Position >= query.PositionFrom!.Value);

            return await petsQuery
                .ToPagedList(
                query.Page,
                query.PageSize,
                cancellationToken);
        }
    }
}