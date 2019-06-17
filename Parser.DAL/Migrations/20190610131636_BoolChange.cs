using Microsoft.EntityFrameworkCore.Migrations;

namespace Parser.DAL.Migrations
{
    public partial class BoolChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ViewSetting",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ViewSetting",
                table: "Users",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
