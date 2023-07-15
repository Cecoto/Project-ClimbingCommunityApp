﻿// <auto-generated />
using System;
using ClimbingCommunity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClimbingCommunity.Data.Migrations
{
    [DbContext(typeof(ClimbingCommunityDbContext))]
    partial class ClimbingCommunityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasComment("User age");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("User firstname");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasComment("User gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("User lastname");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)")
                        .HasComment("User profile picture");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasComment("Here we save the userRole in the application.");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("Administrator");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ClimberSpeciality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Entity identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("Speciality type");

                    b.HasKey("Id");

                    b.ToTable("ClimberSpecialities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Boulderer"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Rope climber"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Free solo climber"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Speed climber"
                        },
                        new
                        {
                            Id = 5,
                            Name = "All rounder"
                        });
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ClimbingTrip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Globaly unique identifier");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()")
                        .HasComment("Date and time user creted the entity");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Destination or location of the climbing trip.");

                    b.Property<int>("Duration")
                        .HasColumnType("int")
                        .HasComment("Duration of the trip in days.");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasComment("Property for soft delete.");

                    b.Property<string>("OrganizatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Photo of the climbing trip place");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Title of the trip");

                    b.Property<int>("TripTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizatorId");

                    b.HasIndex("TripTypeId");

                    b.ToTable("ClimbingTrips");

                    b.HasData(
                        new
                        {
                            Id = new Guid("69c4f3b2-2e74-4f3a-a6aa-ffe4b591abe0"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Destination = "France, Fontainebleau",
                            Duration = 10,
                            IsActive = true,
                            OrganizatorId = "930cb0dc-0c2c-4e74-a885-d93f862588fb",
                            PhotoUrl = "\"~/images/ClimbingTrips/Font.jpg\"",
                            Title = "First Climbing Trip",
                            TripTypeId = 1
                        },
                        new
                        {
                            Id = new Guid("03e1a942-7c20-400b-a776-77f45bb0c558"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Destination = "South Afrika, Capetown",
                            Duration = 20,
                            IsActive = true,
                            OrganizatorId = "930cb0dc-0c2c-4e74-a885-d93f862588fb",
                            PhotoUrl = "~/images/ClimbingTrips/Rocklands.webp",
                            Title = "Second Climbing Trip",
                            TripTypeId = 1
                        },
                        new
                        {
                            Id = new Guid("709c5e92-5818-4eed-b98e-fb5f1b0876df"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Destination = "Spain, Mallorca",
                            Duration = 5,
                            IsActive = true,
                            OrganizatorId = "930cb0dc-0c2c-4e74-a885-d93f862588fb",
                            PhotoUrl = "~/images/ClimbingTrips/Mallorca.jpg",
                            Title = "Third Climbing Trip",
                            TripTypeId = 3
                        });
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Entity identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasComment("Name/Title of the level.");

                    b.HasKey("Id");

                    b.ToTable("Levels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Begginer"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Intermediate"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Advanced"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Pro"
                        });
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Target", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Entity identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("Show us the target of the training");

                    b.HasKey("Id");

                    b.ToTable("Target");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Endurence"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Strenght"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Power-Endurance"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Conditioning"
                        },
                        new
                        {
                            Id = 5,
                            Name = "General fitness"
                        });
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Training", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Globaly unique identifier");

                    b.Property<string>("CoachId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()")
                        .HasComment("Date and time user creted the entity");

                    b.Property<int>("Duration")
                        .HasColumnType("int")
                        .HasComment("Duration of the training in hours.");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Location where will be the training - gym, climbing gym etc.");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Photo of the climbing training willbe/Gym picture");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasComment("Price for the training");

                    b.Property<int>("TargetId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("Title of the training.");

                    b.Property<bool?>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true)
                        .HasComment("Property for soft delete action.");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("TargetId");

                    b.ToTable("Trainings");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4f9e7b2f-c085-4fea-b064-3efbbf6beab2"),
                            CoachId = "3c0bd0ac-9f64-4444-ab43-a31a1886462b",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Duration = 2,
                            Location = "Bulgaria, Sofia",
                            PhotoUrl = "~/images/Traingings/Sofia.jpg",
                            Price = 25.00m,
                            TargetId = 2,
                            Title = "First training"
                        },
                        new
                        {
                            Id = new Guid("558d2f08-7cbd-4b95-a661-cd6c8320cf35"),
                            CoachId = "3c0bd0ac-9f64-4444-ab43-a31a1886462b",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Duration = 3,
                            Location = "Austria, Innsbruck",
                            PhotoUrl = "~/images/Traingings/Innsbruck.jpg",
                            Price = 25.00m,
                            TargetId = 2,
                            Title = "Second training"
                        },
                        new
                        {
                            Id = new Guid("7e614d87-fe30-40d2-9214-4aea1ce7e98f"),
                            CoachId = "749a78a0-ce39-4a27-b9f7-e83ec7642768",
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Duration = 1,
                            Location = "Spain, Madrid",
                            PhotoUrl = "~/images/Traingings/Madrid.jpg",
                            Price = 25.00m,
                            TargetId = 2,
                            Title = "Third training"
                        });
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.TripClimber", b =>
                {
                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClimberId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TripId", "ClimberId");

                    b.HasIndex("ClimberId");

                    b.ToTable("TripsClimbers");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.TripType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Trip identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("Trip name/title");

                    b.HasKey("Id");

                    b.ToTable("TripTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bouldering"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Deep water soloing"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Rope-climbing"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Climber", b =>
                {
                    b.HasBaseType("ClimbingCommunity.Data.Models.ApplicationUser");

                    b.Property<int>("ClimberSpecialityId")
                        .HasColumnType("int");

                    b.Property<int>("ClimbingExperience")
                        .HasColumnType("int")
                        .HasComment("Climbing experience that have the climber.");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.HasIndex("ClimberSpecialityId");

                    b.HasIndex("LevelId");

                    b.HasDiscriminator().HasValue("Climber");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Coach", b =>
                {
                    b.HasBaseType("ClimbingCommunity.Data.Models.ApplicationUser");

                    b.Property<int>("CoachingExperience")
                        .HasColumnType("int")
                        .HasComment("Year of coaching experience that coach have.");

                    b.HasDiscriminator().HasValue("Coach");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ClimbingTrip", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.Climber", "Organizator")
                        .WithMany()
                        .HasForeignKey("OrganizatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClimbingCommunity.Data.Models.TripType", "TripType")
                        .WithMany()
                        .HasForeignKey("TripTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizator");

                    b.Navigation("TripType");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Photo", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.ApplicationUser", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Training", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.Coach", "Coach")
                        .WithMany("Trainings")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClimbingCommunity.Data.Models.Target", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coach");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.TripClimber", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.Climber", "Climber")
                        .WithMany("ClimbingTrips")
                        .HasForeignKey("ClimberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ClimbingCommunity.Data.Models.ClimbingTrip", "ClimbingTrip")
                        .WithMany("Climbers")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Climber");

                    b.Navigation("ClimbingTrip");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClimbingCommunity.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Climber", b =>
                {
                    b.HasOne("ClimbingCommunity.Data.Models.ClimberSpeciality", "ClimberSpeciality")
                        .WithMany()
                        .HasForeignKey("ClimberSpecialityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClimbingCommunity.Data.Models.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClimberSpeciality");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.ClimbingTrip", b =>
                {
                    b.Navigation("Climbers");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Climber", b =>
                {
                    b.Navigation("ClimbingTrips");
                });

            modelBuilder.Entity("ClimbingCommunity.Data.Models.Coach", b =>
                {
                    b.Navigation("Trainings");
                });
#pragma warning restore 612, 618
        }
    }
}
