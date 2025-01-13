using CSharpFunctionalExtensions;
using P2Project.Core;
using P2Project.Core.Errors;

namespace P2Project.Volunteers.Domain.ValueObjects.Pets
{
    public record HealthInfo
    {
        public const string DB_COLUMN_WEIGHT = "weight";
        public const string DB_COLUMN_HEIGHT = "height";
        public const string DB_COLUMN_IS_CASTRATED = "is_castrated";
        public const string DB_COLUMN_IS_VACCINATED = "is_vaccinated";
        public const string DB_COLUMN_HEALTH_DESCRIPTION = "health_description";

        private HealthInfo(double weight,
                           double height,
                           bool isCastrated,
                           bool isVaccinated,
                           string? healthDescription)
        {
            Weight = weight;
            Height = height;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            HealthDescription = healthDescription;
        }
        public double Weight { get; }
        public double Height { get; }
        public bool IsCastrated { get; }
        public bool IsVaccinated { get; }
        public string? HealthDescription { get; }

        public static Result<HealthInfo, Error> Create(
            double weight,
            double height,
            bool isCastrated,
            bool isVaccinated,
            string? healthDescription)
        {
            if (Constants.MIN_WEIGHT_HEIGHT >= weight || weight >= Constants.MAX_WEIGHT_HEIGHT)
                return Errors.General.ValueIsInvalid(nameof(weight));
            
            if (Constants.MIN_WEIGHT_HEIGHT >= height || height >= Constants.MAX_WEIGHT_HEIGHT)
                return Errors.General.ValueIsInvalid(nameof(height));
            
            if (string.IsNullOrWhiteSpace(healthDescription))
                return Errors.General.ValueIsInvalid(nameof(healthDescription));

            var newHealthInfo = new HealthInfo(
                weight, height, isCastrated, isVaccinated, healthDescription);

            return newHealthInfo;
        }
    }
}
