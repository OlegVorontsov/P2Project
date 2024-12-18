﻿using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public class AssistanceStatus
    {
        private static List<AssistanceStatus> _statusList = [NeedsHelp,
                                                               NeedsFood,
                                                               OnMedication,
                                                               LooksForHome,
                                                               FoundHome];
        private AssistanceStatus() { }
        private AssistanceStatus(string status)
        {
            Status = status;
            _statusList.Add(this);
        }
        public string Status { get; } = default!;
        public static AssistanceStatus NeedsHelp { get; } = default!;
        public static AssistanceStatus NeedsFood { get; } = default!;
        public static AssistanceStatus OnMedication { get; } = default!;
        public static AssistanceStatus LooksForHome { get; } = default!;
        public static AssistanceStatus FoundHome { get; } = default!;
        public static Result<AssistanceStatus, Error> Create(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Errors.General.ValueIsInvalid(nameof(Status));

            var newStatus = new AssistanceStatus(status);

            return newStatus;
        }
    }
}
