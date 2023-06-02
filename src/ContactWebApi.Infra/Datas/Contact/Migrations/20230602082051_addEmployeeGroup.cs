using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactWebApi.Infra.Datas.Contact.Migrations
{
    public partial class addEmployeeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GroupId",
                table: "Employees",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeGroups_GroupId",
                table: "Employees",
                column: "GroupId",
                principalTable: "EmployeeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeGroups_GroupId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeGroups");

            migrationBuilder.DropIndex(
                name: "IX_Employees_GroupId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Employees");
        }
    }
}
