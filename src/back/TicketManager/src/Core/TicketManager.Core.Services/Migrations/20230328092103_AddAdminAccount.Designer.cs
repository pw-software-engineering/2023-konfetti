﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TicketManager.Core.Services.DataAccess;

#nullable disable

namespace TicketManager.Core.Services.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20230328092103_AddAdminAccount")]
    partial class AddAdminAccount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TicketManager.Core.Domain.Accounts.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("RowVersion");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TicketManager.Core.Domain.Events.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<Guid>("OrganizerId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("RowVersion");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("TicketManager.Core.Domain.Organizer.Organizer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("DateModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("RowVersion");

                    b.Property<string>("TaxId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("TaxIdType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Organizers");
                });

            modelBuilder.Entity("TicketManager.Core.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("RowVersion");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TicketManager.Core.Domain.Events.Event", b =>
                {
                    b.OwnsMany("TicketManager.Core.Domain.Events.Sector", "Sectors", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.Property<int>("NumberOfColumns")
                                .HasColumnType("integer");

                            b1.Property<int>("NumberOfRows")
                                .HasColumnType("integer");

                            b1.Property<int>("PriceInSmallestUnit")
                                .HasColumnType("integer");

                            b1.HasKey("EventId", "Name");

                            b1.HasIndex("EventId");

                            b1.ToTable("Sector");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("Sectors");
                });
#pragma warning restore 612, 618
        }
    }
}
