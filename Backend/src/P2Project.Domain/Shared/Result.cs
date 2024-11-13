
namespace P2Project.Domain.Shared
{
    public class Result
    {
        public Error Error { get; set; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException();
            if (isSuccess == false && error == Error.None)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static implicit operator Result(Error error) =>
            new(false, error);
    }
    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public Result(TValue value, bool isSucess, Error error)
            : base(isSucess, error)
        {
            _value = value;
        }
        public TValue Value => IsSuccess ? _value
            : throw new InvalidOperationException("The value of failure result" +
                "can't be accessed");
        public static Result<TValue> Success(TValue value) =>
            new (value, true, Error.None);
        public static Result<TValue> Failure(Error error) =>
            new (default!, false, error);

        public static implicit operator Result<TValue>(TValue value) =>
            new(value, true, null);
        public static implicit operator Result<TValue>(Error error) =>
            new(default!, false, error);
    }
}
