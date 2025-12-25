using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlCode.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Curly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    TotalProblems = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsCustom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyPlanId = table.Column<int>(type: "int", nullable: false),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyPlanId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CompletedProblems = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyPlanProgresses_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanItemProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyPlanProgressId = table.Column<int>(type: "int", nullable: false),
                    StudyPlanItemId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanItemProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanItemProgresses_StudyPlanItems_StudyPlanItemId",
                        column: x => x.StudyPlanItemId,
                        principalTable: "StudyPlanItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyPlanItemProgresses_StudyPlanProgresses_StudyPlanProgressId",
                        column: x => x.StudyPlanProgressId,
                        principalTable: "StudyPlanProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProblemId",
                table: "Comments",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItemProgresses_StudyPlanItemId",
                table: "StudyPlanItemProgresses",
                column: "StudyPlanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItemProgresses_StudyPlanProgressId_StudyPlanItemId",
                table: "StudyPlanItemProgresses",
                columns: new[] { "StudyPlanProgressId", "StudyPlanItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_ProblemId",
                table: "StudyPlanItems",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_StudyPlanId_DayNumber_Order",
                table: "StudyPlanItems",
                columns: new[] { "StudyPlanId", "DayNumber", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgresses_StudyPlanId_UserId",
                table: "StudyPlanProgresses",
                columns: new[] { "StudyPlanId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanProgresses_UserId",
                table: "StudyPlanProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_UserId",
                table: "StudyPlans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Problems_ProblemId",
                table: "Comments",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Problems_ProblemId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "StudyPlanItemProgresses");

            migrationBuilder.DropTable(
                name: "StudyPlanItems");

            migrationBuilder.DropTable(
                name: "StudyPlanProgresses");

            migrationBuilder.DropTable(
                name: "StudyPlans");

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
    }
}
