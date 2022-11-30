using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beca.PlaylistInfo.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 1, "The one with that big park.", "Playlist 1" });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 2, "The one with the cathedral that was never really finished.", "Playlist 2" });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 3, "The one with that big tower.", "Playlist 3" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 1, "The most visited urban park in the United States.", 1, "Song 1" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 2, "A 102-story skyscraper located in Midtown Manhattan.", 1, "Song 3" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 3, "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", 2, "Song 4" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 4, "The the finest example of railway architecture in Belgium.", 2, "Song 5" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 5, "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", 3, "Song 6" });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Description", "PlaylistId", "Title" },
                values: new object[] { 6, "The world's largest museum.", 3, "Song 7" });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlaylistId",
                table: "Songs",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Playlists");
        }
    }
}
