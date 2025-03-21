﻿using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Domain.ValueObjects.Pets
{
    public class Position : ValueObject
    {
        public const string DB_COLUMN_POSITION = "position";
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

        public static implicit operator int (Position position) =>
            position.Value;

        // задает объект уникальности класса
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
