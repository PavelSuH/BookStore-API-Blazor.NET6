using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore.API.Migrations
{
    public partial class AddUsersAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f987b51-4404-4f91-824a-1cb53921832a", "76299bfa-be4a-448d-b1c9-289608f27145", "User", "USER" },
                    { "956fe1e9-131f-414f-9267-2743f23bca6e", "d27fae1f-5355-4d13-803c-1a453e261a79", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4601852b-7be2-49b4-a983-ec1ee209d80d", 0, "c141547c-fe15-46a7-b4eb-6bbd00910bc6", "AdminBook@mail.com", false, "System", "Admin", false, null, "ADMINBOOK@MAIL.COM", "ADMINMOOK@MAIL.COM", "AQAAAAEAACcQAAAAEOCDBkej4zPMUEHgXaFJ5/J03mxXxu/pCD6ECEh7r1lujmWv+Z01Oxk1iV9Ux1aEEw==", null, false, "adfbd6e7-9661-415d-b24d-212533be471a", false, "AdminBook@mail.com" },
                    { "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0", 0, "d1aac659-157c-448e-bbd9-b8d25424d332", "UserBook@mail.com", false, "System", "User", false, null, "USERBOOK@MAIL.COM", "USERMOOK@MAIL.COM", "AQAAAAEAACcQAAAAEM70E808ELBR9okYvA0E6rw1qnPv2Ifq+7k1VdDG2E2pUThMklMwm93AsH/Xa9JQ8w==", null, false, "304081e6-c043-49ca-bf8a-057e9a1914da", false, "UserBook@mail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "956fe1e9-131f-414f-9267-2743f23bca6e", "4601852b-7be2-49b4-a983-ec1ee209d80d" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0f987b51-4404-4f91-824a-1cb53921832a", "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "956fe1e9-131f-414f-9267-2743f23bca6e", "4601852b-7be2-49b4-a983-ec1ee209d80d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0f987b51-4404-4f91-824a-1cb53921832a", "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f987b51-4404-4f91-824a-1cb53921832a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "956fe1e9-131f-414f-9267-2743f23bca6e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4601852b-7be2-49b4-a983-ec1ee209d80d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0");
        }
    }
}
