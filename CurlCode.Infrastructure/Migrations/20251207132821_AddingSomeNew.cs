using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlCode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingSomeNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes");

            migrationBuilder.DropIndex(
                name: "IX_ProblemLikes_ProblemId",
                table: "ProblemLikes");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "UserProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "UserProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "UserProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Work",
                table: "UserProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DislikesCount",
                table: "Solutions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Problems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DislikesCount",
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

            migrationBuilder.CreateTable(
                name: "ProblemComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemComments_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemDislikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemDislikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemDislikes_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolutionDislikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolutionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionDislikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolutionDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolutionDislikes_Solutions_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemLikes_ProblemId_UserId",
                table: "ProblemLikes",
                columns: new[] { "ProblemId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemComments_ProblemId",
                table: "ProblemComments",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemComments_UserId",
                table: "ProblemComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemDislikes_ProblemId_UserId",
                table: "ProblemDislikes",
                columns: new[] { "ProblemId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemDislikes_UserId",
                table: "ProblemDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionDislikes_SolutionId_UserId",
                table: "SolutionDislikes",
                columns: new[] { "SolutionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolutionDislikes_UserId",
                table: "SolutionDislikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes");

            migrationBuilder.DropTable(
                name: "ProblemComments");

            migrationBuilder.DropTable(
                name: "ProblemDislikes");

            migrationBuilder.DropTable(
                name: "SolutionDislikes");

            migrationBuilder.DropIndex(
                name: "IX_ProblemLikes_ProblemId_UserId",
                table: "ProblemLikes");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "University",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Work",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DislikesCount",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "DislikesCount",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Problems");

            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "Problems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemLikes_ProblemId",
                table: "ProblemLikes",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemLikes_AspNetUsers_UserId",
                table: "ProblemLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
