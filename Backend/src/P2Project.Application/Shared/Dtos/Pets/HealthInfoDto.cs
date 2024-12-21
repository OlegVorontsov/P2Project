namespace P2Project.Application.Shared.Dtos.Pets;

public record HealthInfoDto(
    double Weight,
    double Height,
    bool IsCastrated,
    bool IsVaccinated,
    string? HealthDescription);