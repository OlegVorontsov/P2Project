using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record SerialNumber
    {
        public static SerialNumber First() => new(1);
        public int Value { get; }
        public SerialNumber(int value)
        {
            Value = value;
        }
        public static Result<SerialNumber, Error> Create(int value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("serial number");

            return new SerialNumber(value);
        }
    }
}
