﻿// <auto-generated />
using System;
using HavucDent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HavucDent.Infrastructure.Migrations
{
    [DbContext(typeof(HavucDbContext))]
    [Migration("20241003133734_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppointmentProduct", b =>
                {
                    b.Property<int>("AppointmentsId")
                        .HasColumnType("int");

                    b.Property<int>("UsedProductsId")
                        .HasColumnType("int");

                    b.HasKey("AppointmentsId", "UsedProductsId");

                    b.HasIndex("UsedProductsId");

                    b.ToTable("AppointmentProducts", (string)null);
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AssistantId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalFee")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AssistantId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.LeaveHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LeaveDaysUsed")
                        .HasColumnType("int");

                    b.Property<DateTime>("LeaveEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LeaveStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("LeaveHistories");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TcKimlikNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.SalaryHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("SalaryPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SalaryHistories");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AnnualLeaveDays")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("SalaryPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SgkNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TcKimlikNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Admin", b =>
                {
                    b.HasBaseType("HavucDent.Domain.Entities.User");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Assistant", b =>
                {
                    b.HasBaseType("HavucDent.Domain.Entities.User");

                    b.HasDiscriminator().HasValue("Assistant");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Doctor", b =>
                {
                    b.HasBaseType("HavucDent.Domain.Entities.User");

                    b.Property<decimal>("CommissionRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProductCommissionRate")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("Doctor");
                });

            modelBuilder.Entity("AppointmentProduct", b =>
                {
                    b.HasOne("HavucDent.Domain.Entities.Appointment", null)
                        .WithMany()
                        .HasForeignKey("AppointmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HavucDent.Domain.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("UsedProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("HavucDent.Domain.Entities.Assistant", "Assistant")
                        .WithMany("AssignedAppointments")
                        .HasForeignKey("AssistantId");

                    b.HasOne("HavucDent.Domain.Entities.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HavucDent.Domain.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assistant");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.LeaveHistory", b =>
                {
                    b.HasOne("HavucDent.Domain.Entities.User", "User")
                        .WithMany("LeaveHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.SalaryHistory", b =>
                {
                    b.HasOne("HavucDent.Domain.Entities.User", "User")
                        .WithMany("SalaryHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.User", b =>
                {
                    b.Navigation("LeaveHistories");

                    b.Navigation("SalaryHistories");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Assistant", b =>
                {
                    b.Navigation("AssignedAppointments");
                });

            modelBuilder.Entity("HavucDent.Domain.Entities.Doctor", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
