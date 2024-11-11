﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;
using P2Project.Domain.ValueObjects;

namespace P2Project.Infrastructure.Configurations
{
    public class PetPhotConfiguration : IEntityTypeConfiguration<PetPhoto>
    {
        public void Configure(EntityTypeBuilder<PetPhoto> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(p => p.Path)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                   .HasColumnName("path");

            builder.Property(p => p.IsMain)
                   .IsRequired()
                   .HasColumnName("is_main");
        }
    }
}