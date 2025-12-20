using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppodev.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixServiceSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "Services",
                newName: "Duration");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Services",
                newName: "DurationMinutes");
        }
    }
}
