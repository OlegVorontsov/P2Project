using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record Position
    {
        public static Position First() => new(1);
        public int Value { get; }
        public Position(int value) => Value = value;
        public Result<Position, Error> Forward() =>
            Create(Value + 1);
        public Result<Position, Error> Back() =>
            Create(Value - 1);

        public static Result<Position, Error> Create(int value)
        {
            if (value < 1)
                return Errors.General.ValueIsInvalid("position");

            return new Position(value);
        }
    }
}
