﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RickApps.UploadFilesMVC.Data;

#nullable disable

namespace RickApps.UploadFilesMVC.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20211202173057_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RickApps.UploadFilesMVC.Models.Item", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemID"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ItemID");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("RickApps.UploadFilesMVC.Models.Photo", b =>
                {
                    b.Property<int>("PhotoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhotoID"), 1L, 1);

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.HasKey("PhotoID");

                    b.HasIndex("ItemID");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("RickApps.UploadFilesMVC.Models.Photo", b =>
                {
                    b.HasOne("RickApps.UploadFilesMVC.Models.Item", null)
                        .WithMany("Photos")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RickApps.UploadFilesMVC.Models.Item", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
