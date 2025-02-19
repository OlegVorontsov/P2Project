using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects;

public class Content : ValueObject
{
    private Content(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;

    public static Result<Content, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Errors.General.ValueIsInvalid(nameof(Content));
        
        return new Content(value);
    }
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}