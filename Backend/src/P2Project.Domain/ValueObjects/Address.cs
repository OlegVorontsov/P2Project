using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record Address
    {
        private Address(string region,
                        string city,
                        string street,
                        string house,
                        string? floor,
                        string? apartment)
        {
            Region = region;
            City = city;
            Street = street;
            House = house;
            Floor = floor;
            Apartment = apartment;
        }
        public string Region { get; } = default!;
        public string City { get; } = default!;
        public string Street { get; } = default!;
        public string House { get; } = default!;
        public string? Floor { get; }
        public string? Apartment { get; }
        public static Result<Address> Create(string region,
                                             string city,
                                             string street,
                                             string house,
                                             string? floor,
                                             string? apartment)
        {
            if (string.IsNullOrEmpty(region))
                return "Region can't be empty";
            if (string.IsNullOrEmpty(city))
                return "City can't be empty";
            if (string.IsNullOrEmpty(street))
                return "Street can't be empty";
            if (string.IsNullOrEmpty(house))
                return "House can't be empty";

            var newAddress = new Address(region, city, street, house,
                                         floor, apartment);

            return newAddress;
        }
    }
}
