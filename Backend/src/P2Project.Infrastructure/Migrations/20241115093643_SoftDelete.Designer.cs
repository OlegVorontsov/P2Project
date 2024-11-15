﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using P2Project.Infrastructure;

#nullable disable

namespace P2Project.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241115093643_SoftDelete")]
    partial class SoftDelete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("P2Project.Domain.PetManagment.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ceated_at");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<double>("Height")
                        .HasMaxLength(10)
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<double>("Weight")
                        .HasMaxLength(10)
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("AssistanceStatus", "P2Project.Domain.PetManagment.Entities.Pet.AssistanceStatus#AssistanceStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("assistance_status");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "P2Project.Domain.PetManagment.Entities.Pet.Color#Color", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("color");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "P2Project.Domain.PetManagment.Entities.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HealthInfo", "P2Project.Domain.PetManagment.Entities.Pet.HealthInfo#HealthInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(300)
                                .HasColumnType("character varying(300)")
                                .HasColumnName("health_info");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("NickName", "P2Project.Domain.PetManagment.Entities.Pet.NickName#NickName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("nick_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OwnerPhoneNumber", "P2Project.Domain.PetManagment.Entities.Pet.OwnerPhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<bool?>("IsMain")
                                .HasColumnType("boolean")
                                .HasColumnName("is_main");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("owner_phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SpeciesBreed", "P2Project.Domain.PetManagment.Entities.Pet.SpeciesBreed#SpeciesBreed", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.ValueObjects.PetPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("path");

                    b.Property<Guid>("pet_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.HasKey("Id")
                        .HasName("pk_pet_photo");

                    b.HasIndex("pet_id")
                        .HasDatabaseName("ix_pet_photo_pet_id");

                    b.ToTable("pet_photo", (string)null);
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Age")
                        .HasMaxLength(10)
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gender");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registered_date");

                    b.Property<double>("YearsOfExperience")
                        .HasColumnType("double precision")
                        .HasColumnName("years_of_experience");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "P2Project.Domain.PetManagment.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "P2Project.Domain.PetManagment.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(300)
                                .HasColumnType("character varying(300)")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "P2Project.Domain.PetManagment.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("last_name");

                            b1.Property<string>("SecondName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("second_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("P2Project.Domain.SpeciesManagment.Entities.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("breed_id")
                        .HasColumnType("uuid")
                        .HasColumnName("breed_id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "P2Project.Domain.SpeciesManagment.Entities.Breed.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_breed");

                    b.HasIndex("breed_id")
                        .HasDatabaseName("ix_breed_breed_id");

                    b.ToTable("breed", (string)null);
                });

            modelBuilder.Entity("P2Project.Domain.SpeciesManagment.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "P2Project.Domain.SpeciesManagment.Species.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.Entities.Pet", b =>
                {
                    b.HasOne("P2Project.Domain.PetManagment.Volunteer", "Volunteer")
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("P2Project.Domain.PetManagment.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Apartment")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Floor")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("House")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("address");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");
                        });

                    b.OwnsOne("P2Project.Domain.PetManagment.ValueObjects.PetAssistanceDetails", "AssistanceDetails", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("assistance_details");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("P2Project.Domain.PetManagment.ValueObjects.AssistanceDetail", "AssistanceDetails", b2 =>
                                {
                                    b2.Property<Guid>("PetAssistanceDetailsPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("AccountNumber")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("character varying(300)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("PetAssistanceDetailsPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("assistance_details");

                                    b2.WithOwner()
                                        .HasForeignKey("PetAssistanceDetailsPetId")
                                        .HasConstraintName("fk_pets_pets_pet_assistance_details_pet_id");
                                });

                            b1.Navigation("AssistanceDetails");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("AssistanceDetails");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.ValueObjects.PetPhoto", b =>
                {
                    b.HasOne("P2Project.Domain.PetManagment.Entities.Pet", null)
                        .WithMany("PetPhotos")
                        .HasForeignKey("pet_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pet_photo_pets_pet_id");
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.Volunteer", b =>
                {
                    b.OwnsOne("P2Project.Domain.PetManagment.ValueObjects.VolunteerAssistanceDetails", "AssistanceDetails", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("assistance_details");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("P2Project.Domain.PetManagment.ValueObjects.AssistanceDetail", "AssistanceDetails", b2 =>
                                {
                                    b2.Property<Guid>("VolunteerAssistanceDetailsVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("AccountNumber")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("character varying(300)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("VolunteerAssistanceDetailsVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("assistance_details");

                                    b2.WithOwner()
                                        .HasForeignKey("VolunteerAssistanceDetailsVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_volunteer_assistance_details_volunteer_id");
                                });

                            b1.Navigation("AssistanceDetails");
                        });

                    b.OwnsOne("P2Project.Domain.PetManagment.ValueObjects.VolunteerPhoneNumbers", "PhoneNumbers", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("phone_numbers");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("P2Project.Domain.PetManagment.ValueObjects.PhoneNumber", "PhoneNumbers", b2 =>
                                {
                                    b2.Property<Guid>("VolunteerPhoneNumbersVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool?>("IsMain")
                                        .IsRequired()
                                        .HasColumnType("boolean");

                                    b2.Property<string>("Value")
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("VolunteerPhoneNumbersVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("phone_numbers");

                                    b2.WithOwner()
                                        .HasForeignKey("VolunteerPhoneNumbersVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_volunteer_phone_numbers_volunteer_id");
                                });

                            b1.Navigation("PhoneNumbers");
                        });

                    b.OwnsOne("P2Project.Domain.PetManagment.ValueObjects.VolunteerSocialNetworks", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("social_networks");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("P2Project.Domain.PetManagment.ValueObjects.SocialNetwork", "SocialNetworks", b2 =>
                                {
                                    b2.Property<Guid>("VolunteerSocialNetworksVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Link")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("VolunteerSocialNetworksVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("social_networks");

                                    b2.WithOwner()
                                        .HasForeignKey("VolunteerSocialNetworksVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_volunteer_social_networks_volunteer_id");
                                });

                            b1.Navigation("SocialNetworks");
                        });

                    b.Navigation("AssistanceDetails");

                    b.Navigation("PhoneNumbers")
                        .IsRequired();

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("P2Project.Domain.SpeciesManagment.Entities.Breed", b =>
                {
                    b.HasOne("P2Project.Domain.SpeciesManagment.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("breed_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_breed_species_breed_id");
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.Entities.Pet", b =>
                {
                    b.Navigation("PetPhotos");
                });

            modelBuilder.Entity("P2Project.Domain.PetManagment.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("P2Project.Domain.SpeciesManagment.Species", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}
