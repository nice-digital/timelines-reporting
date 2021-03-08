﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NICE.Timelines.Models.Database;

namespace NICE.Timelines.Migrations
{
    [DbContext(typeof(TimelinesContext))]
    partial class TimelinesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Latin1_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NICE.Timelines.Models.Database.TimelineTask", b =>
                {
                    b.Property<int>("TimelineTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Acid")
                        .HasColumnType("int")
                        .HasColumnName("ACID");

                    b.Property<DateTime?>("ActualDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ClickUpId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateTypeDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("DateTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TimelineTaskId");

                    b.ToTable("TimelineTask");
                });
#pragma warning restore 612, 618
        }
    }
}
