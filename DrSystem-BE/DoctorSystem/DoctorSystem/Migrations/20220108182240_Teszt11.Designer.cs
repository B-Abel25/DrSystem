﻿// <auto-generated />
using System;
using DoctorSystem.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoctorSystem.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    [Migration("20220108182240_Teszt11")]
    partial class Teszt11
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("DoctorSystem.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("MedNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("Member")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ModifyDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
