using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class AssistanceDetail : ValueObject
    {
        private AssistanceDetail(string name,
                                 string description,
                                 string accountNumber)
        {
            Name = name;
            Description = description;
            AccountNumber = accountNumber;
        }
        public string Name { get; } = default!;
        public string Description { get; } = default!;
        public string AccountNumber { get; } = default!;
        public static Result<AssistanceDetail> Create(string name,
                                                      string description,
                                                      string accountNumber)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<AssistanceDetail>("Name can't be empty");
            }

            var newAssistanceDetail = new AssistanceDetail(name, description,
                                                           accountNumber);

            return Result.Success(newAssistanceDetail);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
