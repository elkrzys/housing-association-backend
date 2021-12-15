﻿// <auto-generated />
using System;
using HousingAssociation.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HousingAssociation.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211215192016_RemoveBuildngIdFromIssue")]
    partial class RemoveBuildngIdFromIssue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AnnouncementBuilding", b =>
                {
                    b.Property<int>("AnnouncementsId")
                        .HasColumnType("integer")
                        .HasColumnName("announcements_id");

                    b.Property<int>("TargetBuildingsId")
                        .HasColumnType("integer")
                        .HasColumnName("target_buildings_id");

                    b.HasKey("AnnouncementsId", "TargetBuildingsId")
                        .HasName("pk_announcements_buildings");

                    b.HasIndex("TargetBuildingsId")
                        .HasDatabaseName("ix_announcements_buildings_target_buildings_id");

                    b.ToTable("announcements_buildings");
                });

            modelBuilder.Entity("DocumentUser", b =>
                {
                    b.Property<int>("DocumentsId")
                        .HasColumnType("integer")
                        .HasColumnName("documents_id");

                    b.Property<int>("ReceiversId")
                        .HasColumnType("integer")
                        .HasColumnName("receivers_id");

                    b.HasKey("DocumentsId", "ReceiversId")
                        .HasName("pk_users_documents");

                    b.HasIndex("ReceiversId")
                        .HasDatabaseName("ix_users_documents_receivers_id");

                    b.ToTable("users_documents");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("city");

                    b.Property<string>("District")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("district");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("street");

                    b.HasKey("Id")
                        .HasName("pk_addresses");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    b.Property<DateTimeOffset?>("Cancelled")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cancelled");

                    b.Property<string>("Content")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<DateTimeOffset?>("Expired")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expired");

                    b.Property<int?>("PreviousAnnouncementId")
                        .HasColumnType("integer")
                        .HasColumnName("previous_announcement_id");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_announcements");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_announcements_author_id");

                    b.HasIndex("PreviousAnnouncementId")
                        .HasDatabaseName("ix_announcements_previous_announcement_id");

                    b.ToTable("announcements");

                    b.HasCheckConstraint("CK_announcements_type_Enum", "type IN ('Issue', 'Announcement', 'Alert')");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AddressId")
                        .HasColumnType("integer")
                        .HasColumnName("address_id");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("number");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_buildings");

                    b.HasIndex("AddressId")
                        .HasDatabaseName("ix_buildings_address_id");

                    b.ToTable("buildings");

                    b.HasCheckConstraint("CK_buildings_type_Enum", "type IN ('Block', 'House')");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int?>("DaysToExpire")
                        .HasColumnType("integer")
                        .HasColumnName("days_to_expire");

                    b.Property<string>("Filepath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("filepath");

                    b.Property<string>("Md5")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("md5");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_documents");

                    b.HasIndex("AuthorId")
                        .IsUnique()
                        .HasDatabaseName("ix_documents_author_id");

                    b.ToTable("documents");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    b.Property<DateTimeOffset?>("Cancelled")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cancelled");

                    b.Property<string>("Content")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("content");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int?>("PreviousIssueId")
                        .HasColumnType("integer")
                        .HasColumnName("previous_issue_id");

                    b.Property<DateTimeOffset?>("Resolved")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("resolved");

                    b.Property<int>("SourceLocalId")
                        .HasColumnType("integer")
                        .HasColumnName("source_local_id");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_issues");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_issues_author_id");

                    b.HasIndex("PreviousIssueId")
                        .HasDatabaseName("ix_issues_previous_issue_id");

                    b.HasIndex("SourceLocalId")
                        .HasDatabaseName("ix_issues_source_local_id");

                    b.ToTable("issues");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Local", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<float?>("Area")
                        .HasColumnType("real")
                        .HasColumnName("area");

                    b.Property<int>("BuildingId")
                        .HasColumnType("integer")
                        .HasColumnName("building_id");

                    b.Property<bool>("IsFullyOwned")
                        .HasColumnType("boolean")
                        .HasColumnName("is_fully_owned");

                    b.Property<string>("Number")
                        .HasColumnType("text")
                        .HasColumnName("number");

                    b.HasKey("Id")
                        .HasName("pk_locals");

                    b.HasIndex("BuildingId")
                        .HasDatabaseName("ix_locals_building_id");

                    b.ToTable("locals");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("text")
                        .HasColumnName("reason_revoked");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text")
                        .HasColumnName("replaced_by_token");

                    b.Property<DateTimeOffset?>("Revoked")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("revoked");

                    b.Property<string>("Token")
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_tokens");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_tokens_user_id");

                    b.ToTable("refresh_tokens");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_enabled");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("last_name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("phone_number");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");

                    b.HasCheckConstraint("CK_users_role_Enum", "role IN ('Admin', 'Worker', 'Resident')");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.UserCredentials", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password_hash");

                    b.HasKey("UserId")
                        .HasName("pk_credentials");

                    b.ToTable("credentials");
                });

            modelBuilder.Entity("LocalUser", b =>
                {
                    b.Property<int>("ResidedLocalsId")
                        .HasColumnType("integer")
                        .HasColumnName("resided_locals_id");

                    b.Property<int>("ResidentsId")
                        .HasColumnType("integer")
                        .HasColumnName("residents_id");

                    b.HasKey("ResidedLocalsId", "ResidentsId")
                        .HasName("pk_locals_residents");

                    b.HasIndex("ResidentsId")
                        .HasDatabaseName("ix_locals_residents_residents_id");

                    b.ToTable("locals_residents");
                });

            modelBuilder.Entity("LocalUser1", b =>
                {
                    b.Property<int>("OwnedLocalsId")
                        .HasColumnType("integer")
                        .HasColumnName("owned_locals_id");

                    b.Property<int>("OwnersId")
                        .HasColumnType("integer")
                        .HasColumnName("owners_id");

                    b.HasKey("OwnedLocalsId", "OwnersId")
                        .HasName("pk_locals_owners");

                    b.HasIndex("OwnersId")
                        .HasDatabaseName("ix_locals_owners_owners_id");

                    b.ToTable("locals_owners");
                });

            modelBuilder.Entity("AnnouncementBuilding", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Announcement", null)
                        .WithMany()
                        .HasForeignKey("AnnouncementsId")
                        .HasConstraintName("fk_announcements_buildings_announcements_announcements_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.Building", null)
                        .WithMany()
                        .HasForeignKey("TargetBuildingsId")
                        .HasConstraintName("fk_announcements_buildings_buildings_target_buildings_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentUser", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Document", null)
                        .WithMany()
                        .HasForeignKey("DocumentsId")
                        .HasConstraintName("fk_users_documents_documents_documents_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("ReceiversId")
                        .HasConstraintName("fk_users_documents_users_receivers_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Announcement", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("fk_announcements_users_author_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.Announcement", "PreviousAnnouncement")
                        .WithMany()
                        .HasForeignKey("PreviousAnnouncementId")
                        .HasConstraintName("fk_announcements_announcements_previous_announcement_id");

                    b.Navigation("Author");

                    b.Navigation("PreviousAnnouncement");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Building", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .HasConstraintName("fk_buildings_addresses_address_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Document", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.User", "Author")
                        .WithOne("Document")
                        .HasForeignKey("HousingAssociation.DataAccess.Entities.Document", "AuthorId")
                        .HasConstraintName("fk_documents_users_author_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Issue", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.User", "Author")
                        .WithMany("Issues")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("fk_issues_users_author_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.Issue", "PreviousIssue")
                        .WithMany()
                        .HasForeignKey("PreviousIssueId")
                        .HasConstraintName("fk_issues_issues_previous_issue_id");

                    b.HasOne("HousingAssociation.DataAccess.Entities.Local", "Local")
                        .WithMany("Issues")
                        .HasForeignKey("SourceLocalId")
                        .HasConstraintName("fk_issues_locals_source_local_id")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Local");

                    b.Navigation("PreviousIssue");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Local", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Building", "Building")
                        .WithMany("Locals")
                        .HasForeignKey("BuildingId")
                        .HasConstraintName("fk_locals_buildings_building_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.RefreshToken", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_refresh_tokens_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.UserCredentials", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.User", "User")
                        .WithOne("UserCredentials")
                        .HasForeignKey("HousingAssociation.DataAccess.Entities.UserCredentials", "UserId")
                        .HasConstraintName("fk_credentials_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LocalUser", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Local", null)
                        .WithMany()
                        .HasForeignKey("ResidedLocalsId")
                        .HasConstraintName("fk_locals_residents_locals_resided_locals_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("ResidentsId")
                        .HasConstraintName("fk_locals_residents_users_residents_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LocalUser1", b =>
                {
                    b.HasOne("HousingAssociation.DataAccess.Entities.Local", null)
                        .WithMany()
                        .HasForeignKey("OwnedLocalsId")
                        .HasConstraintName("fk_locals_owners_locals_owned_locals_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousingAssociation.DataAccess.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("OwnersId")
                        .HasConstraintName("fk_locals_owners_users_owners_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Building", b =>
                {
                    b.Navigation("Locals");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.Local", b =>
                {
                    b.Navigation("Issues");
                });

            modelBuilder.Entity("HousingAssociation.DataAccess.Entities.User", b =>
                {
                    b.Navigation("Document");

                    b.Navigation("Issues");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserCredentials");
                });
#pragma warning restore 612, 618
        }
    }
}
