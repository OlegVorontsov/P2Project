namespace P2Project.Core.Dtos.Pets;

public record HealthInfoDto(
    double Weight,
    double Height,
    bool IsCastrated,
    bool IsVaccinated,
    string? HealthDescription);