using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMajorsAndRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorID = table.Column<Guid>(type: "uuid", nullable: false),
                    MajorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MajorDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorID);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomName = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionID", "PermissionName" },
                values: new object[,]
                {
                    { new Guid("57f9b08e-9106-9025-748d-2aa5d0266d5e"), "major.read" },
                    { new Guid("657121f5-d300-167c-327f-e8c2f7d293f7"), "room.create" },
                    { new Guid("6ea448b1-c8cf-93b2-2db6-3eab482791f3"), "major.delete" },
                    { new Guid("73d41289-9d5e-58e5-2d4f-f7ebde39cbd2"), "room.delete" },
                    { new Guid("85850ab8-cd77-d39d-8134-2e7f7f385736"), "room.read" },
                    { new Guid("94c8d0a2-5979-2168-92fa-698550546a93"), "major.create" },
                    { new Guid("d819a74d-a233-a2ea-25fd-3ead333f4ac8"), "room.update" },
                    { new Guid("ec060fd8-539d-b9b1-9b77-13c7957fa9f5"), "major.update" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Majors_MajorName",
                table: "Majors",
                column: "MajorName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomName",
                table: "Rooms",
                column: "RoomName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("57f9b08e-9106-9025-748d-2aa5d0266d5e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("657121f5-d300-167c-327f-e8c2f7d293f7"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("6ea448b1-c8cf-93b2-2db6-3eab482791f3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("73d41289-9d5e-58e5-2d4f-f7ebde39cbd2"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("85850ab8-cd77-d39d-8134-2e7f7f385736"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("94c8d0a2-5979-2168-92fa-698550546a93"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("d819a74d-a233-a2ea-25fd-3ead333f4ac8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionID",
                keyValue: new Guid("ec060fd8-539d-b9b1-9b77-13c7957fa9f5"));
        }
    }
}
