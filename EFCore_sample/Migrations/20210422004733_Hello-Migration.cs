using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore_sample.Migrations
{
    public partial class HelloMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guild",
                columns: table => new
                {
                    GuildId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guild", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Player_Guild_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guild",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    TestOwnerId = table.Column<int>(type: "int", nullable: false),
                    TestCreatorId = table.Column<int>(type: "int", nullable: true),
                    RecoveredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestroyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Item_Player_TestCreatorId",
                        column: x => x.TestCreatorId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Player_TestOwnerId",
                        column: x => x.TestOwnerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemOption",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Str = table.Column<int>(type: "int", nullable: false),
                    Dex = table.Column<int>(type: "int", nullable: false),
                    Hp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOption", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ItemOption_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemReview",
                columns: table => new
                {
                    ItemReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReview", x => x.ItemReviewId);
                    table.ForeignKey(
                        name: "FK_ItemReview_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_TestCreatorId",
                table: "Item",
                column: "TestCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_TestOwnerId",
                table: "Item",
                column: "TestOwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemReview_ItemId",
                table: "ItemReview",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "Index_Person_Name",
                table: "Player",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_GuildId",
                table: "Player",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemOption");

            migrationBuilder.DropTable(
                name: "ItemReview");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Guild");
        }
    }
}
