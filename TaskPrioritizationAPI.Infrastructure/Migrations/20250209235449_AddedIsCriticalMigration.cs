using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPrioritizationAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsCriticalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCritical",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCritical",
                table: "Tasks");
        }
    }
}
