using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "MaNguoiDung", "DangHoatDong", "HoTen", "MatKhau", "TenDangNhap", "VaiTro" },
                values: new object[] { 4, true, "Nhân viên kho", "123456", "ware01", "Warehouse" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 4);
        }
    }
}
