﻿// <auto-generated />
using System;
using DataTech.System.Versioning.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataTech.System.Versioning.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230410080128_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataTech.System.Versioning.Models.Domain.AppModule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppSystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UpdateIndex")
                        .HasColumnType("int");

                    b.Property<int>("VersionIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AppModules");
                });

            modelBuilder.Entity("DataTech.System.Versioning.Models.Domain.AppSystem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReleaseIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AppSystems");
                });

            modelBuilder.Entity("DataTech.System.Versioning.Models.Domain.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });
#pragma warning restore 612, 618
        }
    }
}