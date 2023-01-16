﻿// <auto-generated />
using System;
using Ciomag_Andreea_Museum.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ciomag_Andreea_Museum.Migrations
{
    [DbContext(typeof(MuseumContext))]
    [Migration("20230104191610_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.12");

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Artist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Artist", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Exhibit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArtistID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExhibitionID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Movement")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ArtistID");

                    b.HasIndex("ExhibitionID");

                    b.ToTable("Exhibit", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Exhibition", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("FinishDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("GalleryID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("GalleryID");

                    b.ToTable("Exhibition", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Gallery", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Closing")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Opening")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Gallery", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Visit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GalleryID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("GalleryID");

                    b.ToTable("Visit", (string)null);
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Exhibit", b =>
                {
                    b.HasOne("Ciomag_Andreea_Museum.Models.Artist", "Artist")
                        .WithMany("Exhibits")
                        .HasForeignKey("ArtistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ciomag_Andreea_Museum.Models.Exhibition", "Exhibition")
                        .WithMany("Exhibits")
                        .HasForeignKey("ExhibitionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Exhibition");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Exhibition", b =>
                {
                    b.HasOne("Ciomag_Andreea_Museum.Models.Gallery", "Gallery")
                        .WithMany("Exhibitions")
                        .HasForeignKey("GalleryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gallery");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Visit", b =>
                {
                    b.HasOne("Ciomag_Andreea_Museum.Models.Client", "Client")
                        .WithMany("Visits")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ciomag_Andreea_Museum.Models.Gallery", "Gallery")
                        .WithMany("Visits")
                        .HasForeignKey("GalleryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Gallery");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Artist", b =>
                {
                    b.Navigation("Exhibits");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Client", b =>
                {
                    b.Navigation("Visits");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Exhibition", b =>
                {
                    b.Navigation("Exhibits");
                });

            modelBuilder.Entity("Ciomag_Andreea_Museum.Models.Gallery", b =>
                {
                    b.Navigation("Exhibitions");

                    b.Navigation("Visits");
                });
#pragma warning restore 612, 618
        }
    }
}