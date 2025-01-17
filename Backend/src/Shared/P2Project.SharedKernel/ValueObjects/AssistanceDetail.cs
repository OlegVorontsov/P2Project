using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects
{
    public record AssistanceDetail
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
        public static Result<AssistanceDetail, Error> Create(
                                                      string name,
                                                      string description,
                                                      string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.Errors.General.ValueIsInvalid(nameof(Name));
            if (string.IsNullOrWhiteSpace(description))
                return Errors.Errors.General.ValueIsInvalid(nameof(Description));
            if (string.IsNullOrWhiteSpace(accountNumber))
                return Errors.Errors.General.ValueIsInvalid(nameof(AccountNumber));

            var newAssistanceDetail = new AssistanceDetail(name, description,
                                                           accountNumber);

            return newAssistanceDetail;
        }
    }
}
