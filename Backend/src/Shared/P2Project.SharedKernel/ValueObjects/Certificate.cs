using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects;

public record Certificate
{
    public string Name { get; }
    public string Value { get; } 

    private Certificate() { }
    private Certificate(string name, string description)
    {
        Name = name;
        Value = description; 
    }

    public static Result<Certificate, Error> Create(string name, string description)
    {
        bool isInvalidRequisite = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description);

        if (isInvalidRequisite)
            return Errors.Errors.General.ValueIsInvalid(nameof(Name));;

        return new Certificate(name, description);
    }
}