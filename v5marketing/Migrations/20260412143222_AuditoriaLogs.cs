using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace v5marketing.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioLogin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Entidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLogs");
        }
    }
}
