﻿using CSharpFunctionalExtensions;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Domain.PetManagment.ValueObjects.Pets
{
    public record SpeciesBreed
    {
        public const string DB_COLUMN_SPECIES_ID = "species_id";
        public const string DB_COLUMN_BREED_ID = "breed_id";
        public SpeciesBreed(SpeciesId speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }
        public SpeciesId SpeciesId { get; }
        public Guid BreedId { get; }
    }
}
