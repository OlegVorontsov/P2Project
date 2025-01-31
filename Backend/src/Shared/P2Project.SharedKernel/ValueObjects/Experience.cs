using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects;

public record Experience
{
    public const string DB_COLUMN_GRADE = "experience";
    private Experience(int value)
    {
        Value = value;
    }
    public int Value { get; }
    
    public static Result<Experience, Error> Create(int experience)
    {
        if (Constants.MIN_EXPERIENCE >= experience || experience >= Constants.MAX_EXPERIENCE)
            return Errors.Errors.General.ValueIsInvalid(nameof(experience));

        var newExperience = new Experience(experience);

        return newExperience;
    }
}