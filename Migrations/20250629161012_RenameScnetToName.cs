using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MP_Backend.Migrations
{
    /// <inheritdoc />
    public partial class RenameScnetToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Scent",
                table: "ProductVariants",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductVariants",
                newName: "Scent");
        }
    }
}
