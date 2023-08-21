﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Riduttore.Server.Migrations
{
    [DbContext(typeof(RiduttoreDbContext))]
    [Migration("20230806101507_VideoEntity")]
    partial class VideoEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Command")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NativePhysicalVaultPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("NativeSizeReadable")
                        .HasColumnType("TEXT");

                    b.Property<string>("NativeVaultPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProcessedPhysicalVaultPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProcessedSizeReadable")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProcessedVaultPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailPhysicalVaultPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailVaultPath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
