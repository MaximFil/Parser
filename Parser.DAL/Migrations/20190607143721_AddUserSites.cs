using Microsoft.EntityFrameworkCore.Migrations;

namespace Parser.DAL.Migrations
{
    public partial class AddUserSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSite_Sites_SiteId",
                table: "UserSite");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSite_Users_UserId",
                table: "UserSite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSite",
                table: "UserSite");

            migrationBuilder.RenameTable(
                name: "UserSite",
                newName: "UserSites");

            migrationBuilder.RenameIndex(
                name: "IX_UserSite_SiteId",
                table: "UserSites",
                newName: "IX_UserSites_SiteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSites",
                table: "UserSites",
                columns: new[] { "UserId", "SiteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSites_Sites_SiteId",
                table: "UserSites",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSites_Users_UserId",
                table: "UserSites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSites_Sites_SiteId",
                table: "UserSites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSites_Users_UserId",
                table: "UserSites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSites",
                table: "UserSites");

            migrationBuilder.RenameTable(
                name: "UserSites",
                newName: "UserSite");

            migrationBuilder.RenameIndex(
                name: "IX_UserSites_SiteId",
                table: "UserSite",
                newName: "IX_UserSite_SiteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSite",
                table: "UserSite",
                columns: new[] { "UserId", "SiteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSite_Sites_SiteId",
                table: "UserSite",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSite_Users_UserId",
                table: "UserSite",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
