using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElizaFlixAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "filmes",
                columns: table => new
                {
                    sigla = table.Column<string>(type: "text", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    dir_Filmes = table.Column<string[]>(type: "text[]", nullable: false),
                    dir_Thumb_Wid = table.Column<string>(type: "text", nullable: false),
                    dir_Thumb_Heid = table.Column<string>(type: "text", nullable: false),
                    quantidade_eps = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filmes", x => x.sigla);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filmes");
        }
    }
}
