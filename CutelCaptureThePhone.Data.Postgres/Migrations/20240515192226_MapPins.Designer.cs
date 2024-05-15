﻿// <auto-generated />
using System;
using CutelCaptureThePhone.Data.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CutelCaptureThePhone.Data.Postgres.Migrations
{
    [DbContext(typeof(CutelCaptureThePhoneDbContext))]
    [Migration("20240515192226_MapPins")]
    partial class MapPins
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.BlacklistEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Interpretation")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("NumberBlacklist");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.Capture", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FromNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Captures");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.MapPin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Lat")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Long")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("MapPins");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Pin")
                        .HasColumnType("bigint");

                    b.Property<string>("RegisteredFromNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Pin")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("citext");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.WhitelistEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Interpretation")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Value", "Interpretation")
                        .IsUnique();

                    b.ToTable("NumberWhitelist");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.Capture", b =>
                {
                    b.HasOne("CutelCaptureThePhone.Data.Postgres.Entities.Player", "Player")
                        .WithMany("Captures")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CutelCaptureThePhone.Data.Postgres.Entities.Player", b =>
                {
                    b.Navigation("Captures");
                });
#pragma warning restore 612, 618
        }
    }
}
