using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticalNineteen.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentTableWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    varchar10 = table.Column<string>(name: "varchar(10)", type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "FirstName", "Gender", "LastName", "MiddleName", "varchar(10)" },
                values: new object[,]
                {
                    { 1, "Rajkot", new DateTime(2002, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bhavin", "M", "Kareliya", null, "1231231231" },
                    { 2, "Rajkot", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jil", "M", "Patel", null, "1231231231" },
                    { 3, "Rajkot", new DateTime(1999, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vipul", "M", "Kumar", null, "1231231231" },
                    { 4, "Rajkot", new DateTime(2000, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jay", "M", "Gohel", null, "1231231231" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
