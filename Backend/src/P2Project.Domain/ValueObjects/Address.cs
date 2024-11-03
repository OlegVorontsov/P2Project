﻿using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class Address
    {
        private Address(string region,
                        string city,
                        string street,
                        string house,
                        string floor,
                        string apartment)
        {
            Region = region;
            City = city;
            Street = street;
            House = house;
            Floor = floor;
            Apartment = apartment;
        }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Floor { get; set; }
        public string Apartment { get; set; }
        public static Result<Address> Create(string region,
                                             string city,
                                             string street,
                                             string house,
                                             string floor,
                                             string apartment)
        {
            if (string.IsNullOrEmpty(region))
                return "Region can't be empty";

            var newAddress = new Address(region, city, street, house,
                                         floor, apartment);

            return newAddress;
        }
    }
}
