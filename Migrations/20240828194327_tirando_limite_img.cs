using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace megev.Migrations
{
    /// <inheritdoc />
    public partial class tirando_limite_img : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "Usuario",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "Usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");
        }
    }
}
