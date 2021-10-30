using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Migrations
{
    public partial class UserDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "CorporationUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorporationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pib = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CorporationCity = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CorporationAddress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporationUser", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CorporationUser_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorporationUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalUser", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_PersonalUser_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("8c349e7b-1c97-486d-aa2e-e58205d11577"), "Serbia" },
                    { new Guid("ff0c9396-7c4c-4bf5-a12e-6aa79c272413"), "US" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { new Guid("987268e5-f880-4f81-b1bf-5b9704604e26"), "Admin" },
                    { new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), "Regular user" }
                });

            migrationBuilder.InsertData(
                table: "CorporationUser",
                columns: new[] { "UserId", "CorporationAddress", "CorporationCity", "CorporationName", "CountryId", "Email", "IsActive", "Pib", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "Kisacka 25", "Novi Sad", "Stark", new Guid("8c349e7b-1c97-486d-aa2e-e58205d11577"), "stark@gmail.com", true, "515731", new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), "+38160111222", "Stark" },
                    { new Guid("9346b8c4-1b3b-435f-9c35-35de3a76fcf9"), "unknown", "Washington DC", "National Bank", new Guid("ff0c9396-7c4c-4bf5-a12e-6aa79c272413"), "nat_bank@gmail.com", true, "51516", new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), "+38165555113", "NationalBank" }
                });

            migrationBuilder.InsertData(
                table: "PersonalUser",
                columns: new[] { "UserId", "CountryId", "Email", "FirstName", "IsActive", "LastName", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("ce593d02-c615-4af6-a794-c450b79e9b4d"), new Guid("8c349e7b-1c97-486d-aa2e-e58205d11577"), "djolex3211@gmail.com", "Djordje", true, "Stefanovic", new Guid("987268e5-f880-4f81-b1bf-5b9704604e26"), "+381628192354", "djolex" },
                    { new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), new Guid("8c349e7b-1c97-486d-aa2e-e58205d11577"), "marko@gmail.com", "Marko", true, "Markovic", new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), "+3816965555555", "markoMarkovic" },
                    { new Guid("194df880-d4ce-4997-96c9-878102eb6e0e"), new Guid("ff0c9396-7c4c-4bf5-a12e-6aa79c272413"), "nevena@gmail.com", "Nevena", true, "Nikolic", new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), "+381691234567", "nikolicNN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorporationUser_CountryId",
                table: "CorporationUser",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationUser_RoleId",
                table: "CorporationUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CorporationUser_Username",
                table: "CorporationUser",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_CountryId",
                table: "PersonalUser",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_RoleId",
                table: "PersonalUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_Username",
                table: "PersonalUser",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorporationUser");

            migrationBuilder.DropTable(
                name: "PersonalUser");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
