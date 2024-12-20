﻿namespace P2Project.Application.Shared.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string NickName { get; init; } = string.Empty;
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string HealthInfo { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string House { get; init; } = string.Empty;
    public string? Floor { get; init; } = string.Empty;
    public string? Apartment { get; init; } = string.Empty;
    public double Weight { get; init; }
    public double Height { get; init; }
    public string OwnerPhoneNumber { get; init; } = string.Empty;
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public string DateOfBirth { get; init; } = string.Empty;
    public string AssistanceStatus { get; init; } = string.Empty;
    public string CreatedAt { get; init; } = string.Empty;
    public int Position { get; init; }
    public IEnumerable<AssistanceDetailDto> AssistanceDetails { get; set; } = default!;
    public IEnumerable<PetPhotoDto> Photos { get; set; } = default!;
}