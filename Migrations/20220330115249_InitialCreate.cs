using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMind.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    PriorityID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Active = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.PriorityID);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsClosed = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Trackers",
                columns: table => new
                {
                    TrackerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackers", x => x.TrackerID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    Salt = table.Column<string>(type: "VARCHAR(24)", maxLength: 24, nullable: true),
                    PasswordHash = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IssueID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CreatorID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    AssigneeID = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusID = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackerID = table.Column<int>(type: "INTEGER", nullable: false),
                    PriorityID = table.Column<int>(type: "INTEGER", nullable: false),
                    IsClosed = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.IssueID);
                    table.ForeignKey(
                        name: "FK_Issue_Priorities_PriorityID",
                        column: x => x.PriorityID,
                        principalTable: "Priorities",
                        principalColumn: "PriorityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issue_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issue_Trackers_TrackerID",
                        column: x => x.TrackerID,
                        principalTable: "Trackers",
                        principalColumn: "TrackerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issue_Users_AssigneeID",
                        column: x => x.AssigneeID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issue_Users_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issue_AssigneeID",
                table: "Issue",
                column: "AssigneeID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_CreatorID",
                table: "Issue",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_PriorityID",
                table: "Issue",
                column: "PriorityID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_StatusID",
                table: "Issue",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_TrackerID",
                table: "Issue",
                column: "TrackerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Trackers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
