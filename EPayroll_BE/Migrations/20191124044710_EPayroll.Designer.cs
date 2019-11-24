﻿// <auto-generated />
using System;
using EPayroll_BE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EPayroll_BE.Migrations
{
    [DbContext(typeof(EPayrollContext))]
    [Migration("20191124044710_EPayroll")]
    partial class EPayroll
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EPayroll_BE.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmployeeCode")
                        .IsRequired();

                    b.Property<bool>("IsRemove");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("EPayroll_BE.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int>("Age");

                    b.Property<bool>("Gender");

                    b.Property<string>("IdentifyNumber");

                    b.Property<string>("Name");

                    b.Property<int>("PositionId");

                    b.Property<int>("SalaryLevelId");

                    b.Property<int>("SalaryModeId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PositionId");

                    b.HasIndex("SalaryLevelId");

                    b.HasIndex("SalaryModeId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount");

                    b.Property<int>("PaySlipId");

                    b.Property<int>("PayTypeId");

                    b.HasKey("Id");

                    b.HasIndex("PaySlipId");

                    b.HasIndex("PayTypeId");

                    b.ToTable("PayItem");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("PayDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("PayPeriod");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PaySlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId");

                    b.Property<string>("Name");

                    b.Property<int>("PayPeriodId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PayPeriodId");

                    b.ToTable("PaySlip");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("PayTypeCategoryId");

                    b.HasKey("Id");

                    b.HasIndex("PayTypeCategoryId");

                    b.ToTable("PayType");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayTypeAmount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount");

                    b.Property<int>("PayTypeId");

                    b.Property<int>("SalaryLevelId");

                    b.HasKey("Id");

                    b.HasIndex("PayTypeId");

                    b.HasIndex("SalaryLevelId");

                    b.ToTable("PayTypeAmount");
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayTypeCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PayTypeCategory");
                });

            modelBuilder.Entity("EPayroll_BE.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Condition");

                    b.Property<string>("Level");

                    b.Property<int>("Order");

                    b.Property<int>("SalaryTableId");

                    b.HasKey("Id");

                    b.HasIndex("SalaryTableId");

                    b.ToTable("SalaryLevel");
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryMode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Mode");

                    b.HasKey("Id");

                    b.ToTable("SalaryMode");
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalarySheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount");

                    b.Property<string>("Name");

                    b.Property<int>("PaySlipId");

                    b.Property<int>("PayTypeId");

                    b.Property<int>("TotalOverTimeWorking");

                    b.Property<int>("TotalWorking");

                    b.Property<float>("WorkingRate");

                    b.HasKey("Id");

                    b.HasIndex("PaySlipId");

                    b.HasIndex("PayTypeId");

                    b.ToTable("SalarySheet");
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("SalaryTable");
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryTablePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PositionId");

                    b.Property<int>("SalaryTableId");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("SalaryTableId");

                    b.ToTable("SalaryTablePosition");
                });

            modelBuilder.Entity("EPayroll_BE.Models.Employee", b =>
                {
                    b.HasOne("EPayroll_BE.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.SalaryLevel", "SalaryLevel")
                        .WithMany()
                        .HasForeignKey("SalaryLevelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.SalaryMode", "SalaryMode")
                        .WithMany()
                        .HasForeignKey("SalaryModeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayItem", b =>
                {
                    b.HasOne("EPayroll_BE.Models.PaySlip", "PaySlip")
                        .WithMany()
                        .HasForeignKey("PaySlipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.PayType", "PayType")
                        .WithMany()
                        .HasForeignKey("PayTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.PaySlip", b =>
                {
                    b.HasOne("EPayroll_BE.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.PayPeriod", "PayPeriod")
                        .WithMany()
                        .HasForeignKey("PayPeriodId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayType", b =>
                {
                    b.HasOne("EPayroll_BE.Models.PayTypeCategory", "PayTypeCategory")
                        .WithMany()
                        .HasForeignKey("PayTypeCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.PayTypeAmount", b =>
                {
                    b.HasOne("EPayroll_BE.Models.PayType", "PayType")
                        .WithMany()
                        .HasForeignKey("PayTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.SalaryLevel", "SalaryLevel")
                        .WithMany()
                        .HasForeignKey("SalaryLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryLevel", b =>
                {
                    b.HasOne("EPayroll_BE.Models.SalaryTable", "SalaryTable")
                        .WithMany()
                        .HasForeignKey("SalaryTableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalarySheet", b =>
                {
                    b.HasOne("EPayroll_BE.Models.PaySlip", "PaySlip")
                        .WithMany()
                        .HasForeignKey("PaySlipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.PayType", "PayType")
                        .WithMany()
                        .HasForeignKey("PayTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPayroll_BE.Models.SalaryTablePosition", b =>
                {
                    b.HasOne("EPayroll_BE.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPayroll_BE.Models.SalaryTable", "SalaryTable")
                        .WithMany()
                        .HasForeignKey("SalaryTableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
