﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Service_Incidents.Data;

#nullable disable

namespace Service_Incidents.Migrations
{
    [DbContext(typeof(IncidentsDbContext))]
    [Migration("20240612124241_History_Action")]
    partial class History_Action
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Service_Incidents.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Service_Incidents.Models.Incident", b =>
                {
                    b.Property<int>("INCD_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("INCD_ID"));

                    b.Property<string>("INCD_DESC")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("INCD_ENTT_SG_ID")
                        .HasColumnType("int");

                    b.Property<string>("INCD_NUM_TICK")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("INCD_PRIO_ID")
                        .HasColumnType("int");

                    b.Property<int>("INCD_STAT_ID")
                        .HasColumnType("int");

                    b.Property<int>("INCD_TYPE_ID")
                        .HasColumnType("int");

                    b.Property<int>("INCD_UTIL_ID")
                        .HasColumnType("int");

                    b.Property<bool?>("IsSendSms1")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSendSms2")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSendSms3")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSendSms4")
                        .HasColumnType("bit");

                    b.Property<int?>("Motif_id")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("agn_code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("incd_audit")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("incd_date_cloture")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("incd_date_creation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("incd_date_resolution")
                        .HasColumnType("datetime2");

                    b.Property<int?>("niveau_escalade")
                        .HasColumnType("int");

                    b.Property<int?>("pres_id")
                        .HasColumnType("int");

                    b.HasKey("INCD_ID");

                    b.HasIndex("INCD_PRIO_ID");

                    b.HasIndex("INCD_STAT_ID");

                    b.HasIndex("INCD_TYPE_ID");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("Service_Incidents.Models.IncidentHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ChangedBy")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int>("IncidentId")
                        .HasColumnType("int");

                    b.Property<int>("NewStatusId")
                        .HasColumnType("int");

                    b.Property<int>("OldStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IncidentId");

                    b.HasIndex("NewStatusId");

                    b.HasIndex("OldStatusId");

                    b.ToTable("IncidentHistories");
                });

            modelBuilder.Entity("Service_Incidents.Models.Priorite", b =>
                {
                    b.Property<int>("INCD_PRIO_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("INCD_PRIO_ID"));

                    b.Property<string>("PRIO_DESC")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("INCD_PRIO_ID");

                    b.ToTable("Priorites");
                });

            modelBuilder.Entity("Service_Incidents.Models.Statut", b =>
                {
                    b.Property<int>("INCD_STAT_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("INCD_STAT_ID"));

                    b.Property<string>("STAT_DESC")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("INCD_STAT_ID");

                    b.ToTable("Statuts");
                });

            modelBuilder.Entity("Service_Incidents.Models.Types", b =>
                {
                    b.Property<int>("INCD_TYPE_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("INCD_TYPE_ID"));

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("TYPE_DESC")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("INCD_TYPE_ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("Service_Incidents.Models.Incident", b =>
                {
                    b.HasOne("Service_Incidents.Models.Priorite", null)
                        .WithMany()
                        .HasForeignKey("INCD_PRIO_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Service_Incidents.Models.Statut", null)
                        .WithMany()
                        .HasForeignKey("INCD_STAT_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Service_Incidents.Models.Types", null)
                        .WithMany()
                        .HasForeignKey("INCD_TYPE_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Service_Incidents.Models.IncidentHistory", b =>
                {
                    b.HasOne("Service_Incidents.Models.Incident", "Incident")
                        .WithMany()
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Service_Incidents.Models.Statut", "NewStatus")
                        .WithMany()
                        .HasForeignKey("NewStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Service_Incidents.Models.Statut", "OldStatus")
                        .WithMany()
                        .HasForeignKey("OldStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Incident");

                    b.Navigation("NewStatus");

                    b.Navigation("OldStatus");
                });

            modelBuilder.Entity("Service_Incidents.Models.Types", b =>
                {
                    b.HasOne("Service_Incidents.Models.Category", "Category")
                        .WithMany("Types")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Service_Incidents.Models.Category", b =>
                {
                    b.Navigation("Types");
                });
#pragma warning restore 612, 618
        }
    }
}
