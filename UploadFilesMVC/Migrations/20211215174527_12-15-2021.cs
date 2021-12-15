using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RickApps.UploadFilesMVC.Migrations
{
    public partial class _12152021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Photo",
                newName: "LinkToSmallImage");

            migrationBuilder.AddColumn<string>(
                name: "LinkToLargeImage",
                table: "Photo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkToMediumImage",
                table: "Photo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Item",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkToLargeImage",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "LinkToMediumImage",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "LinkToSmallImage",
                table: "Photo",
                newName: "Location");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Item",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
