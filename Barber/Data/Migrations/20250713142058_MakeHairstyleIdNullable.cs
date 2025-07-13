using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barber.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeHairstyleIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HairstyleId",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HairstyleId",
                table: "Appointments",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
