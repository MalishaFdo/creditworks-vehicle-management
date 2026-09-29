using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CWVehicleManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class init_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinWeightKg = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    MaxWeightKg = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    IconKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCategories", x => x.Id);
                    table.CheckConstraint("CK_VehicleCategories_MinWeightKg", "[MinWeightKg] >= 0");
                    table.CheckConstraint("CK_VehicleCategories_Range", "[MaxWeightKg] IS NULL OR [MaxWeightKg] > [MinWeightKg]");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    YearOfManufacture = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.CheckConstraint("CK_Vehicles_WeightKg", "[WeightKg] > 0");
                    table.CheckConstraint("CK_Vehicles_YearOfManufacture", "[YearOfManufacture] >= 1886");
                    table.ForeignKey(
                        name: "FK_Vehicles_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mazda" },
                    { 2, "Mercedes" },
                    { 3, "Honda" },
                    { 4, "Ferrari" },
                    { 5, "Toyota" }
                });

            migrationBuilder.InsertData(
                table: "VehicleCategories",
                columns: new[] { "Id", "IconKey", "MaxWeightKg", "MinWeightKg", "Name" },
                values: new object[,]
                {
                    { 1, "motorcycle", 500m, 0m, "Light" },
                    { 2, "car", 2500m, 500m, "Medium" },
                    { 3, "truck", null, 2500m, "Heavy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCategories_Name",
                table: "VehicleCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ManufacturerId",
                table: "Vehicles",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerName",
                table: "Vehicles",
                column: "OwnerName");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_WeightKg",
                table: "Vehicles",
                column: "WeightKg");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_YearOfManufacture",
                table: "Vehicles",
                column: "YearOfManufacture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleCategories");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
