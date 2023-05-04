using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RottenTomatoes.Migrations
{
    /// <inheritdoc />
    public partial class seriesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Serie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "mainActors",
                table: "Serie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Serie");

            migrationBuilder.DropColumn(
                name: "mainActors",
                table: "Serie");
        }
    }
}
