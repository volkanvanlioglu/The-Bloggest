using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheBloggest.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedById",
                table: "MediaFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles");

            migrationBuilder.RenameTable(
                name: "MediaFiles",
                newName: "MediaLibraries");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_UploadedById",
                table: "MediaLibraries",
                newName: "IX_MediaLibraries_UploadedById");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaLibraries",
                table: "MediaLibraries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaLibraries_AspNetUsers_UploadedById",
                table: "MediaLibraries",
                column: "UploadedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaLibraries_AspNetUsers_UploadedById",
                table: "MediaLibraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaLibraries",
                table: "MediaLibraries");

            migrationBuilder.RenameTable(
                name: "MediaLibraries",
                newName: "MediaFiles");

            migrationBuilder.RenameIndex(
                name: "IX_MediaLibraries_UploadedById",
                table: "MediaFiles",
                newName: "IX_MediaFiles_UploadedById");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedById",
                table: "MediaFiles",
                column: "UploadedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
