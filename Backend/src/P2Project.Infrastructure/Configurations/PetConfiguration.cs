﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                    .HasConversion(
                    id => id.Value,
                    value => PetId.CreatePetId(value));

            builder.ComplexProperty(p => p.NickName, nb =>
            {
                nb.Property(n => n.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName("nick_name");
            });

            // Species todo in 4.4

            builder.ComplexProperty(p => p.Description, db =>
            {
                db.Property(d => d.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_BIG_TEXT_LENGTH)
                  .HasColumnName("description");
            });

            // Breed todo in 4.4

            builder.ComplexProperty(p => p.Color, cb =>
            {
                cb.Property(c => c.Value)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH)
                  .HasColumnName("color");
            });

            builder.ComplexProperty(p => p.HealthInfo, hib =>
            {
                hib.Property(hi => hi.Value)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                  .HasColumnName("health_info");
            });

            builder.OwnsOne(p => p.Address, ab =>
            {
                ab.ToJson();

                ab.Property(a => a.Region)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.City)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Street)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.House)
                  .IsRequired()
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Floor)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                ab.Property(a => a.Apartment)
                  .IsRequired(false)
                  .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
            });

            builder.Property(p => p.Weight)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH)
                   .HasColumnName("weight");

            builder.Property(p => p.Height)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_TINY_TEXT_LENGTH)
                   .HasColumnName("height");

            builder.Property(p => p.OwnerPhoneNumber)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

            builder.Property(p => p.IsCastrated)
                   .HasColumnName("is_castrated");

            builder.Property(p => p.IsVaccinated)
                   .HasColumnName("is_vaccinated");

            builder.Property(p => p.DateOfBirth)
                   .IsRequired()
                   .HasColumnName("date_of_birth")
                   .HasConversion(
                        d => d.ToUniversalTime(),
                        d => DateTime.SpecifyKind(d, DateTimeKind.Local));

            builder.ComplexProperty(p => p.AssistanceStatus, asb =>
            {
                asb.Property(a => a.Status)
                   .IsRequired()
                   .HasColumnName("assistance_status");
            });

            builder.OwnsOne(p => p.AssistanceDetails, adb =>
            {
                adb.ToJson();

                adb.OwnsMany(ad => ad.AssistanceDetails, ab =>
                {
                    ab.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);

                    ab.Property(a => a.Description)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

                    ab.Property(a => a.AccountNumber)
                      .IsRequired()
                      .HasMaxLength(Constants.MAX_SMALL_TEXT_LENGTH);
                });
            });

            builder.HasMany(p => p.PetPhotos)
                   .WithOne(pp => pp.Pet)
                   .HasForeignKey("pet_id")
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
            builder.Navigation(p => p.PetPhotos).AutoInclude();

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnName("ceated_at")
                   .HasConversion(
                        d => d.ToUniversalTime(),
                        d => DateTime.SpecifyKind(d, DateTimeKind.Local));
        }
    }
}
