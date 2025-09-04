using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPTrain.Migrations
{
    /// <inheritdoc />
    public partial class editcoursesmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Courses",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CreatedBy",
                table: "Courses",
                newName: "IX_Courses_CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedById",
                table: "Courses",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedById",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Courses",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CreatedById",
                table: "Courses",
                newName: "IX_Courses_CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_CreatedBy",
                table: "Courses",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
