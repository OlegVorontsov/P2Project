
namespace P2Project.Domain.IDs
{
    public class VolunteerId
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }
        public static VolunteerId NewVolunteerId() => new (Guid.NewGuid());
        public static VolunteerId EmptyVolunteerId() => new(Guid.Empty);
        public static VolunteerId CreateVolunteerId(Guid id) => new(id);

        public static implicit operator Guid(VolunteerId volunteerId)
        {
            ArgumentNullException.ThrowIfNull(volunteerId);
                return volunteerId.Value;
        }
    }
}
