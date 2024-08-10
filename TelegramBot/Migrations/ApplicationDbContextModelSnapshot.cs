﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramBot.Services;

#nullable disable

namespace TelegramBot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramBot.Models.CommandHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Created")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CommandHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
