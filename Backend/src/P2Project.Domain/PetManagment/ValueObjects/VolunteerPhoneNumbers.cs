﻿namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record VolunteerPhoneNumbers
    {
        private VolunteerPhoneNumbers() { }
        public VolunteerPhoneNumbers(IReadOnlyList<PhoneNumber>? phoneNumbers)
        {
            PhoneNumbers = phoneNumbers;
        }
        public IReadOnlyList<PhoneNumber>? PhoneNumbers { get; } = default!;
    }
}
