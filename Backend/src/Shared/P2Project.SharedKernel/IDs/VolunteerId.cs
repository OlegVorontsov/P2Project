﻿namespace P2Project.SharedKernel.IDs
{
    public record VolunteerId
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static VolunteerId New() => new(Guid.NewGuid());
        public static VolunteerId Empty() => new(Guid.Empty);
        public static VolunteerId Create(Guid id) => new(id);

        public static implicit operator Guid(VolunteerId volunteerId)
        {
            ArgumentNullException.ThrowIfNull(volunteerId);
            return volunteerId.Value;
        }
    }
}
