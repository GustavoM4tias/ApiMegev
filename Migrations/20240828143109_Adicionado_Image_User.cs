using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace megev.Migrations
{
    /// <inheritdoc />
    public partial class Adicionado_Image_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "Usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Usuario");
        }
    }
}
