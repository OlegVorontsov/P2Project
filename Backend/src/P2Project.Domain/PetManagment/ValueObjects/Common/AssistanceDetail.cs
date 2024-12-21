using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects.Common
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
                return Errors.General.ValueIsInvalid(nameof(Name));
            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid(nameof(Description));
            if (string.IsNullOrWhiteSpace(accountNumber))
                return Errors.General.ValueIsInvalid(nameof(AccountNumber));

            var newAssistanceDetail = new AssistanceDetail(name, description,
                                                           accountNumber);

            return newAssistanceDetail;
        }
    }
}
