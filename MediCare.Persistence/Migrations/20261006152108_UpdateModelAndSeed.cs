using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCare.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsException",
                table: "User_Permission",
                newName: "IsGranted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsGranted",
                table: "User_Permission",
                newName: "IsException");
        }
    }
}
