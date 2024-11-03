using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class AssistanceDetail
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
                return "Name can't be empty";
            }

            var newAssistanceDetail = new AssistanceDetail(name, description,
                                                           accountNumber);

            return newAssistanceDetail;
        }
    }
}
