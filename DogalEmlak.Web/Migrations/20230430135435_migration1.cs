using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DogalEmlak.Web.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TypeOfProperty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TypeOfUsage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rooms = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SizeOfNet = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SizeOfGross = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DateOfAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfRenewal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    DateOfAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => new { x.PropertyId, x.FileName });
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Authority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.UserId, x.Authority });
                    table.ForeignKey(
                        name: "FK_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfAdded", "FirstName", "LastName", "Locked", "Password", "UserName" },
                values: new object[,]
                {
                    { new Guid("33828faa-f88f-4149-88b7-f2213fece6b3"), new DateTime(2023, 4, 30, 16, 54, 35, 38, DateTimeKind.Local).AddTicks(2925), "Serkan", "Güneşoğlu", false, "FA22FB87B07AE7407F5CEDA208A47996", "serkan123" },
                    { new Guid("ce53da7d-9219-4419-81cc-a741ed6c2627"), new DateTime(2023, 4, 30, 16, 54, 35, 38, DateTimeKind.Local).AddTicks(2784), "Gökhan", "Güneşoğlu", false, "FA22FB87B07AE7407F5CEDA208A47996", "gokhan123" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Authority", "UserId" },
                values: new object[,]
                {
                    { "Staff", new Guid("33828faa-f88f-4149-88b7-f2213fece6b3") },
                    { "Admin", new Guid("ce53da7d-9219-4419-81cc-a741ed6c2627") },
                    { "Manager", new Guid("ce53da7d-9219-4419-81cc-a741ed6c2627") },
                    { "Staff", new Guid("ce53da7d-9219-4419-81cc-a741ed6c2627") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
