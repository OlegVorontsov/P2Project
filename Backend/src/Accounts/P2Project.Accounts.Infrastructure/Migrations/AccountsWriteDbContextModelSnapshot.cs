﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using P2Project.Accounts.Infrastructure.DbContexts;

#nullable disable

namespace P2Project.Accounts.Infrastructure.Migrations
{
    [DbContext(typeof(AccountsWriteDbContext))]
    partial class AccountsWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("accounts")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_role_claims_role_id");

                    b.ToTable("role_claims", "accounts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_claims_user_id");

                    b.ToTable("user_claims", "accounts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_logins_user_id");

                    b.ToTable("user_logins", "accounts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_roles_role_id");

                    b.ToTable("user_roles", "accounts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("user_tokens", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.AdminAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_admin_accounts");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_admin_accounts_user_id");

                    b.ToTable("admin_accounts", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.ParticipantAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("BannedForRequestsUntil")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("banned_for_requests_until");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_participant_accounts");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_participant_accounts_user_id");

                    b.ToTable("participant_accounts", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.VolunteerAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AssistanceDetails")
                        .HasColumnType("text")
                        .HasColumnName("assistance_details");

                    b.Property<string>("Certificates")
                        .HasColumnType("text")
                        .HasColumnName("certificates");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.ComplexProperty<Dictionary<string, object>>("Experience", "P2Project.Accounts.Domain.Accounts.VolunteerAccount.Experience#Experience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("experience");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteer_accounts");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_volunteer_accounts_user_id");

                    b.ToTable("volunteer_accounts", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RefreshSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ExpiresIn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_in");

                    b.Property<Guid>("Jti")
                        .HasColumnType("uuid")
                        .HasColumnName("jti");

                    b.Property<Guid>("RefreshToken")
                        .HasColumnType("uuid")
                        .HasColumnName("refresh_token");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_sessions");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_sessions_user_id");

                    b.ToTable("refresh_sessions", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RolePermission.Permissions.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_permissions");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_permissions_code");

                    b.ToTable("permissions", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RolePermission.RolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid")
                        .HasColumnName("permission_id");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("pk_role_permissions");

                    b.HasIndex("PermissionId")
                        .HasDatabaseName("ix_role_permissions_permission_id");

                    b.ToTable("role_permissions", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RolePermission.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("roles", "accounts");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("Photos")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("photos");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<string>("SocialNetworks")
                        .HasColumnType("text")
                        .HasColumnName("social_networks");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "P2Project.Accounts.Domain.User.FullName#FullName", b1 =>
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
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("users", "accounts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.RolePermission.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_claims_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_claims_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_logins_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.RolePermission.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_roles_role_id");

                    b.HasOne("P2Project.Accounts.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_tokens_users_user_id");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.AdminAccount", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", "User")
                        .WithOne("AdminAccount")
                        .HasForeignKey("P2Project.Accounts.Domain.Accounts.AdminAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_admin_accounts_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.ParticipantAccount", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", "User")
                        .WithOne("ParticipantAccount")
                        .HasForeignKey("P2Project.Accounts.Domain.Accounts.ParticipantAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_participant_accounts_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.Accounts.VolunteerAccount", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", "User")
                        .WithOne("VolunteerAccount")
                        .HasForeignKey("P2Project.Accounts.Domain.Accounts.VolunteerAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_volunteer_accounts_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RefreshSession", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_sessions_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RolePermission.RolePermission", b =>
                {
                    b.HasOne("P2Project.Accounts.Domain.RolePermission.Permissions.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permissions_permissions_permission_id");

                    b.HasOne("P2Project.Accounts.Domain.RolePermission.Roles.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permissions_role_role_id");

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.RolePermission.Roles.Role", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("P2Project.Accounts.Domain.User", b =>
                {
                    b.Navigation("AdminAccount");

                    b.Navigation("ParticipantAccount");

                    b.Navigation("VolunteerAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
