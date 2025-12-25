using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlCode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurlyBre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Problems_ProblemId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProblemId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProblemId",
                table: "Comments",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Problems_ProblemId",
                table: "Comments",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id");
        }
    }
}
