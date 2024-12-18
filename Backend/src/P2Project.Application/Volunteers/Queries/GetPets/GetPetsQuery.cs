﻿using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public record GetPetsQuery(
        string? NickName,
        int Page,
        int PageSize) : IQuery;
}
