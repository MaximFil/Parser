using Microsoft.EntityFrameworkCore.Migrations;

namespace Parser.DAL.Migrations
{
    public partial class AddUserArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticle_Articles_ArticleId",
                table: "UserArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArticle_Users_UserId",
                table: "UserArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticle",
                table: "UserArticle");

            migrationBuilder.RenameTable(
                name: "UserArticle",
                newName: "UserArticles");

            migrationBuilder.RenameIndex(
                name: "IX_UserArticle_ArticleId",
                table: "UserArticles",
                newName: "IX_UserArticles_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_Users_UserId",
                table: "UserArticles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_Users_UserId",
                table: "UserArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles");

            migrationBuilder.RenameTable(
                name: "UserArticles",
                newName: "UserArticle");

            migrationBuilder.RenameIndex(
                name: "IX_UserArticles_ArticleId",
                table: "UserArticle",
                newName: "IX_UserArticle_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticle",
                table: "UserArticle",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticle_Articles_ArticleId",
                table: "UserArticle",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticle_Users_UserId",
                table: "UserArticle",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
