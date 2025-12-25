using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlCode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secondMigOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLike_AspNetUsers_UserId",
                table: "CommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLike_Comments_CommentId",
                table: "CommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLike_AspNetUsers_UserId",
                table: "ProblemLike");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLike_Problems_ProblemId",
                table: "ProblemLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemLike",
                table: "ProblemLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLike",
                table: "CommentLike");

            migrationBuilder.RenameTable(
                name: "ProblemLike",
                newName: "ProblemLikes");

            migrationBuilder.RenameTable(
                name: "CommentLike",
                newName: "CommentLikes");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemLike_UserId",
                table: "ProblemLikes",
                newName: "IX_ProblemLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemLike_ProblemId",
                table: "ProblemLikes",
                newName: "IX_ProblemLikes_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLike_UserId",
                table: "CommentLikes",
                newName: "IX_CommentLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLike_CommentId",
                table: "CommentLikes",
                newName: "IX_CommentLikes_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemLikes",
                table: "ProblemLikes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_AspNetUsers_UserId",
                table: "CommentLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLikes_Problems_ProblemId",
                table: "ProblemLikes",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_AspNetUsers_UserId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLikes_Problems_ProblemId",
                table: "ProblemLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemLikes",
                table: "ProblemLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes");

            migrationBuilder.RenameTable(
                name: "ProblemLikes",
                newName: "ProblemLike");

            migrationBuilder.RenameTable(
                name: "CommentLikes",
                newName: "CommentLike");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemLikes_UserId",
                table: "ProblemLike",
                newName: "IX_ProblemLike_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemLikes_ProblemId",
                table: "ProblemLike",
                newName: "IX_ProblemLike_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLikes_UserId",
                table: "CommentLike",
                newName: "IX_CommentLike_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLikes_CommentId",
                table: "CommentLike",
                newName: "IX_CommentLike_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemLike",
                table: "ProblemLike",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLike",
                table: "CommentLike",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLike_AspNetUsers_UserId",
                table: "CommentLike",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLike_Comments_CommentId",
                table: "CommentLike",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLike_AspNetUsers_UserId",
                table: "ProblemLike",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLike_Problems_ProblemId",
                table: "ProblemLike",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
