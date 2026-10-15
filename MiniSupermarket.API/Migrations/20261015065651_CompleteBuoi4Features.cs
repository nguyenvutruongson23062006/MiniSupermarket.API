using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class CompleteBuoi4Features : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "NguoiDung",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaKhachHang",
                table: "DonHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 1,
                columns: new[] { "Email", "HoTen", "TenDangNhap" },
                values: new object[] { "admin01@minimart.local", "Nguyễn Quản Trị", "admin01" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 2,
                columns: new[] { "Email", "HoTen", "TenDangNhap", "VaiTro" },
                values: new object[] { "admin02@minimart.local", "Trần Giám Đốc", "admin02", "Admin" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 3,
                columns: new[] { "Email", "HoTen", "TenDangNhap" },
                values: new object[] { "cashier01@minimart.local", "Lê Thu Ngân", "cashier01" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 4,
                columns: new[] { "Email", "HoTen", "TenDangNhap", "VaiTro" },
                values: new object[] { "cashier02@minimart.local", "Phạm Bán Hàng", "cashier02", "Cashier" });

            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "MaNguoiDung", "DangHoatDong", "Email", "HoTen", "MatKhau", "TenDangNhap", "VaiTro" },
                values: new object[,]
                {
                    { 5, true, "cashier03@minimart.local", "Hoàng Thu Ngân", "123456", "cashier03", "Cashier" },
                    { 6, true, "cashier04@minimart.local", "Vũ Thị Quầy", "123456", "cashier04", "Cashier" },
                    { 7, true, "cashier05@minimart.local", "Đỗ Bán Lẻ", "123456", "cashier05", "Cashier" },
                    { 8, true, "ware01@minimart.local", "Ngô Quản Kho", "123456", "ware01", "Warehouse" },
                    { 9, true, "ware02@minimart.local", "Bùi Kiểm Kê", "123456", "ware02", "Warehouse" },
                    { 10, true, "ware03@minimart.local", "Dương Thủ Kho", "123456", "ware03", "Warehouse" },
                    { 11, true, "ware04@minimart.local", "Lý Nhập Hàng", "123456", "ware04", "Warehouse" },
                    { 12, true, "admin_backup@minimart.local", "Đặng Hỗ Trợ", "123456", "admin_backup", "Admin" },
                    { 13, true, "cashier06@minimart.local", "Hồ Ca Chiều", "123456", "cashier06", "Cashier" },
                    { 14, true, "ware05@minimart.local", "Trương Vận Chuyển", "123456", "ware05", "Warehouse" },
                    { 15, true, "supervisor@minimart.local", "Mai Giám Sát", "123456", "supervisor", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_SoDienThoai",
                table: "KhachHang",
                column: "SoDienThoai",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang",
                column: "MaKhachHang",
                principalTable: "KhachHang",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_SoDienThoai",
                table: "KhachHang");

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "NguoiDung");

            migrationBuilder.AlterColumn<int>(
                name: "MaKhachHang",
                table: "DonHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 1,
                columns: new[] { "HoTen", "TenDangNhap" },
                values: new object[] { "Quản trị viên", "admin" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 2,
                columns: new[] { "HoTen", "TenDangNhap", "VaiTro" },
                values: new object[] { "Nguyễn Văn Thuận", "cashier1", "Cashier" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 3,
                columns: new[] { "HoTen", "TenDangNhap" },
                values: new object[] { "Trần Văn Nam", "cashier2" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "MaNguoiDung",
                keyValue: 4,
                columns: new[] { "HoTen", "TenDangNhap", "VaiTro" },
                values: new object[] { "Nhân viên kho", "ware01", "Warehouse" });

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_KhachHang_MaKhachHang",
                table: "DonHang",
                column: "MaKhachHang",
                principalTable: "KhachHang",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
