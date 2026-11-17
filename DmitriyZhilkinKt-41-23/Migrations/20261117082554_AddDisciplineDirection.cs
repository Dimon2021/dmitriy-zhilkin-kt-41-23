using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DmitriyZhilkinKt_41_23.Migrations
{
    /// <inheritdoc />
    public partial class AddDisciplineDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "direction",
                table: "Disciplines",
                type: "integer",
                nullable: false,
                defaultValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "direction",
                table: "Disciplines");
        }
    }
}
