using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPayroll_BE.Migrations
{
    public partial class EPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeCode = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsRemove = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayTypeCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayTypeCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryMode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Mode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryMode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsEnable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PayTypeCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayType_PayTypeCategory_PayTypeCategoryId",
                        column: x => x.PayTypeCategoryId,
                        principalTable: "PayTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Condition = table.Column<string>(nullable: true),
                    SalaryTableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryLevel_SalaryTable_SalaryTableId",
                        column: x => x.SalaryTableId,
                        principalTable: "SalaryTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryTablePosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PositionId = table.Column<int>(nullable: false),
                    SalaryTableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryTablePosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryTablePosition_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryTablePosition_SalaryTable_SalaryTableId",
                        column: x => x.SalaryTableId,
                        principalTable: "SalaryTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    IdentifyNumber = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    SalaryModeId = table.Column<int>(nullable: false),
                    SalaryLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_SalaryLevel_SalaryLevelId",
                        column: x => x.SalaryLevelId,
                        principalTable: "SalaryLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_SalaryMode_SalaryModeId",
                        column: x => x.SalaryModeId,
                        principalTable: "SalaryMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayTypeAmount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(nullable: false),
                    SalaryLevelId = table.Column<int>(nullable: false),
                    PayTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayTypeAmount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayTypeAmount_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalTable: "PayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayTypeAmount_SalaryLevel_SalaryLevelId",
                        column: x => x.SalaryLevelId,
                        principalTable: "SalaryLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaySlip",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PayPeriodId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaySlip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaySlip_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaySlip_PayPeriod_PayPeriodId",
                        column: x => x.PayPeriodId,
                        principalTable: "PayPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(nullable: false),
                    PaySlipId = table.Column<int>(nullable: false),
                    PayTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayItem_PaySlip_PaySlipId",
                        column: x => x.PaySlipId,
                        principalTable: "PaySlip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayItem_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalTable: "PayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalarySheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    TotalWorking = table.Column<int>(nullable: false),
                    TotalOverTimeWorking = table.Column<int>(nullable: false),
                    WorkingRate = table.Column<float>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    PaySlipId = table.Column<int>(nullable: false),
                    PayTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalarySheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalarySheet_PaySlip_PaySlipId",
                        column: x => x.PaySlipId,
                        principalTable: "PaySlip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalarySheet_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalTable: "PayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_AccountId",
                table: "Employee",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PositionId",
                table: "Employee",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SalaryLevelId",
                table: "Employee",
                column: "SalaryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SalaryModeId",
                table: "Employee",
                column: "SalaryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayItem_PaySlipId",
                table: "PayItem",
                column: "PaySlipId");

            migrationBuilder.CreateIndex(
                name: "IX_PayItem_PayTypeId",
                table: "PayItem",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaySlip_EmployeeId",
                table: "PaySlip",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaySlip_PayPeriodId",
                table: "PaySlip",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PayType_PayTypeCategoryId",
                table: "PayType",
                column: "PayTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PayTypeAmount_PayTypeId",
                table: "PayTypeAmount",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayTypeAmount_SalaryLevelId",
                table: "PayTypeAmount",
                column: "SalaryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryLevel_SalaryTableId",
                table: "SalaryLevel",
                column: "SalaryTableId");

            migrationBuilder.CreateIndex(
                name: "IX_SalarySheet_PaySlipId",
                table: "SalarySheet",
                column: "PaySlipId");

            migrationBuilder.CreateIndex(
                name: "IX_SalarySheet_PayTypeId",
                table: "SalarySheet",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryTablePosition_PositionId",
                table: "SalaryTablePosition",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryTablePosition_SalaryTableId",
                table: "SalaryTablePosition",
                column: "SalaryTableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayItem");

            migrationBuilder.DropTable(
                name: "PayTypeAmount");

            migrationBuilder.DropTable(
                name: "SalarySheet");

            migrationBuilder.DropTable(
                name: "SalaryTablePosition");

            migrationBuilder.DropTable(
                name: "PaySlip");

            migrationBuilder.DropTable(
                name: "PayType");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "PayPeriod");

            migrationBuilder.DropTable(
                name: "PayTypeCategory");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "SalaryLevel");

            migrationBuilder.DropTable(
                name: "SalaryMode");

            migrationBuilder.DropTable(
                name: "SalaryTable");
        }
    }
}
