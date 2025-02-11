using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects;

public record Content
{
    private Content(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<Content, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Errors.General.ValueIsInvalid(nameof(Content));
        
        return new Content(value);
    } 

    public static implicit operator string(Content content) => content.Value;
}