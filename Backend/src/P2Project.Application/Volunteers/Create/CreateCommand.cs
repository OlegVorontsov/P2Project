﻿using P2Project.Application.Dtos;
using P2Project.Application.Shared;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public record CreateCommand(
                  FullNameDto FullName,
                  int Age,
                  string Gender,
                  string Email,
                  string? Description,
                  IEnumerable<PhoneNumberDto> PhoneNumbers,
                  IEnumerable<SocialNetworkDto>? SocialNetworks,
                  IEnumerable<AssistanceDetailDto>? AssistanceDetails) : ICommand;
}