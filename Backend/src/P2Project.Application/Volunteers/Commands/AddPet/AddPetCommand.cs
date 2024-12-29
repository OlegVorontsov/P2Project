﻿using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Application.Volunteers.Commands.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        Guid SpeciesId,
        Guid BreedId,
        string NickName,
        string? Description,
        string Color,
        HealthInfoDto HealthInfo,
        AddressDto Address,
        PhoneNumberDto OwnerPhoneNumber,
        DateOnly BirthDate,
        string AssistanceStatus,
        AssistanceDetailDto AssistanceDetail) : ICommand;
}
