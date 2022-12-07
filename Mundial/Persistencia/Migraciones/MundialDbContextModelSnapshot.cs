﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mundial.Persistencia;

#nullable disable

namespace Mundial.Persistencia.Migraciones
{
    [DbContext(typeof(MundialDbContext))]
    partial class MundialDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Mundial.Modelo.Pais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Pais");
                });

            modelBuilder.Entity("Mundial.Modelo.Participacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Año")
                        .HasColumnType("int");

                    b.Property<string>("Instancia")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<int?>("PaisId")
                        .HasColumnType("int");

                    b.Property<string>("Sede")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("PaisId");

                    b.ToTable("Participacion");
                });

            modelBuilder.Entity("Mundial.Modelo.Participacion", b =>
                {
                    b.HasOne("Mundial.Modelo.Pais", null)
                        .WithMany("Participaciones")
                        .HasForeignKey("PaisId");
                });

            modelBuilder.Entity("Mundial.Modelo.Pais", b =>
                {
                    b.Navigation("Participaciones");
                });
#pragma warning restore 612, 618
        }
    }
}
