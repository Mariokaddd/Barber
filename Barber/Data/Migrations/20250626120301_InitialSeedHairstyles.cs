using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Barber.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedHairstyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hairstyles",
                columns: new[] { "Id", "Description", "Gender", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, "Къса машинка подстрижка с дължина около 2–3 мм, която акцентира върху формата на главата и е лесна за поддръжка.", 1, "/images/hairstyles/BuzzCut.jpg", "Buzzcut" },
                    { 2, "Дълги плитки за женствен и романтичен вид.", 2, "/images/hairstyles/plitki.jpg", "Момичешки плитки" },
                    { 3, "Стилна прическа с обем отпред и по-къси страни.", 1, "/images/hairstyles/kviff.jpg", "Мъжки квифф" },
                    { 4, "Къса и елегантна боб прическа до брадичката.", 2, "/images/hairstyles/Bob.jpg", "Женски боб" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hairstyles",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
