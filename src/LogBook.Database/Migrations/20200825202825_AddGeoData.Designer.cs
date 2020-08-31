﻿// <auto-generated />
using System;
using LogBook.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LogBook.Database.Migrations
{
    [DbContext(typeof(LogBookDbContext))]
    [Migration("20200825202825_AddGeoData")]
    partial class AddGeoData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("LogBook.Database.Model.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("routes");
                });

            modelBuilder.Entity("LogBook.Database.Model.WayPoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Point>("Location")
                        .HasColumnType("geometry");

                    b.Property<Guid>("RouteId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("waypoints");
                });

            modelBuilder.Entity("LogBook.Database.Model.WayPoint", b =>
                {
                    b.HasOne("LogBook.Database.Model.Route", "Route")
                        .WithMany("WayPoints")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
