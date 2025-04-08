using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSelling.Migrations
{
    /// <inheritdoc />
    public partial class addcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderItems",
                newName: "orderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "orderItemId",
                table: "OrderItems",
                newName: "Id");
        }
    }
}
