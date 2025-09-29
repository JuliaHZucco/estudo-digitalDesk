using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capitulo01.Migrations
{
    /// <inheritdoc />
    public partial class CursoDisciplina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CursoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartamentoID = table.Column<int>(type: "int", nullable: false),
                    DepartamentoID1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.CursoID);
                    table.ForeignKey(
                        name: "FK_Cursos_Departamentos_DepartamentoID1",
                        column: x => x.DepartamentoID1,
                        principalTable: "Departamentos",
                        principalColumn: "DepartamentoID");
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    DisciplinaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartamentoID = table.Column<int>(type: "int", nullable: false),
                    DepartamentoID1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.DisciplinaID);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Departamentos_DepartamentoID1",
                        column: x => x.DepartamentoID1,
                        principalTable: "Departamentos",
                        principalColumn: "DepartamentoID");
                });

            migrationBuilder.CreateTable(
                name: "CursoDisciplina",
                columns: table => new
                {
                    CursoID = table.Column<int>(type: "int", nullable: false),
                    DisciplinaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursoDisciplina", x => new { x.CursoID, x.DisciplinaID });
                    table.ForeignKey(
                        name: "FK_CursoDisciplina_Cursos_CursoID",
                        column: x => x.CursoID,
                        principalTable: "Cursos",
                        principalColumn: "CursoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursoDisciplina_Disciplinas_DisciplinaID",
                        column: x => x.DisciplinaID,
                        principalTable: "Disciplinas",
                        principalColumn: "DisciplinaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CursoDisciplina_DisciplinaID",
                table: "CursoDisciplina",
                column: "DisciplinaID");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_DepartamentoID1",
                table: "Cursos",
                column: "DepartamentoID1");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_DepartamentoID1",
                table: "Disciplinas",
                column: "DepartamentoID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursoDisciplina");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Disciplinas");
        }
    }
}
