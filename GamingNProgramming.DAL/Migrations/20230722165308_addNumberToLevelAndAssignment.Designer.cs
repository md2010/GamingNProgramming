﻿// <auto-generated />
using System;
using GamingNProgramming.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GamingNProgramming.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230722165308_addNumberToLevelAndAssignment")]
    partial class addNumberToLevelAndAssignment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GamingNProgramming.Model.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssignmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("OfferedAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BadgeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasArgs")
                        .HasColumnType("bit");

                    b.Property<bool>("HasBadge")
                        .HasColumnType("bit");

                    b.Property<string>("InitialCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCoding")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMultiSelect")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTimeMeasured")
                        .HasColumnType("bit");

                    b.Property<Guid>("LevelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("Seconds")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BadgeId");

                    b.HasIndex("LevelId");

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Avatar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Badge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Badge");
                });

            modelBuilder.Entity("GamingNProgramming.Model.CoreUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("CoreUsers");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Friend", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Player1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Player2Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Level", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.ToTable("Level");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Map", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProfessorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Map");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Player", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AvatarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefultPoints")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProfessorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("XPs")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("AvatarId");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Professor", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("GamingNProgramming.Model.TestCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssignmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.ToTable("TestCase");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Answer", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Assignment", "Assignment")
                        .WithMany("Answers")
                        .HasForeignKey("AssignmentId");

                    b.Navigation("Assignment");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Assignment", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Badge", "Badge")
                        .WithMany()
                        .HasForeignKey("BadgeId");

                    b.HasOne("GamingNProgramming.Model.Level", "Level")
                        .WithMany("Assignments")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Badge");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("GamingNProgramming.Model.CoreUser", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Friend", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Player", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamingNProgramming.Model.Player", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Level", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Map", "Map")
                        .WithMany("Levels")
                        .HasForeignKey("MapId");

                    b.Navigation("Map");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Map", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Professor", "Professor")
                        .WithMany("Maps")
                        .HasForeignKey("ProfessorId");

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Player", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Avatar", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamingNProgramming.Model.Professor", null)
                        .WithMany("Students")
                        .HasForeignKey("ProfessorId");

                    b.HasOne("GamingNProgramming.Model.CoreUser", "CoreUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");

                    b.Navigation("CoreUser");
                });

            modelBuilder.Entity("GamingNProgramming.Model.TestCase", b =>
                {
                    b.HasOne("GamingNProgramming.Model.Assignment", "Assignment")
                        .WithMany("TestCases")
                        .HasForeignKey("AssignmentId");

                    b.Navigation("Assignment");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Assignment", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("TestCases");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Level", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Map", b =>
                {
                    b.Navigation("Levels");
                });

            modelBuilder.Entity("GamingNProgramming.Model.Professor", b =>
                {
                    b.Navigation("Maps");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
