using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentaCarAPI.Migrations
{
    public partial class seedrole1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("0587031f-01c6-409f-8189-beba853f9c2e"), "6ac14a4b-f761-4629-ad0b-0657f44651aa", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("1e4839f5-d20a-478a-aae7-8dd8724a72e5"), "fc3560a7-36d9-47c5-8f05-1b3a5a6299dc", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0587031f-01c6-409f-8189-beba853f9c2e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1e4839f5-d20a-478a-aae7-8dd8724a72e5"));
        }
    }
}
