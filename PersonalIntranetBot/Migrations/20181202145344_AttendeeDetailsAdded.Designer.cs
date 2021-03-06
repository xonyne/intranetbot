﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PersonalIntranetBot.Models;
using System;

namespace PersonalIntranetBot.Migrations
{
    [DbContext(typeof(DBModelContext))]
    [Migration("20181202145344_AttendeeDetailsAdded")]
    partial class AttendeeDetailsAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalIntranetBot.Models.Attendee", b =>
                {
                    b.Property<int>("AttendeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrentJobCompany");

                    b.Property<string>("CurrentJobTitle");

                    b.Property<string>("DisplayName");

                    b.Property<string>("EducationLocation");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("ImageURL");

                    b.Property<bool>("IsPerson");

                    b.HasKey("AttendeeId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("PersonalIntranetBot.Models.SocialLink", b =>
                {
                    b.Property<int>("SocialLinkId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttendeeId");

                    b.Property<int>("Type");

                    b.Property<string>("URL");

                    b.HasKey("SocialLinkId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("SocialLinks");
                });

            modelBuilder.Entity("PersonalIntranetBot.Models.SocialLink", b =>
                {
                    b.HasOne("PersonalIntranetBot.Models.Attendee")
                        .WithMany("SocialLinks")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
