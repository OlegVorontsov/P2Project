using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects.Pets
{
    public record Address
    {
        public const string DB_COLUMN_REGION = "region";
        public const string DB_COLUMN_CITY = "city";
        public const string DB_COLUMN_STREET = "street";
        public const string DB_COLUMN_HOUSE = "house";
        public const string DB_COLUMN_FLOOR = "floor";
        public const string DB_COLUMN_APARTMENT = "apartment";
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
        public static Result<Address, Error> Create(string region,
                                             string city,
                                             string street,
                                             string house,
                                             string? floor,
                                             string? apartment)
        {
            if (string.IsNullOrEmpty(region))
                return Errors.General.ValueIsInvalid(nameof(Region));
            if (string.IsNullOrEmpty(city))
                return Errors.General.ValueIsInvalid(nameof(City));
            if (string.IsNullOrEmpty(street))
                return Errors.General.ValueIsInvalid(nameof(Street));
            if (string.IsNullOrEmpty(house))
                return Errors.General.ValueIsInvalid(nameof(House));

            var newAddress = new Address(region, city, street, house,
                                         floor, apartment);

            return newAddress;
        }
    }
}
