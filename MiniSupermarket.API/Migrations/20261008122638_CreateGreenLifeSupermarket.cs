using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateGreenLifeSupermarket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DangHoatDong = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhomHang",
                columns: table => new
                {
                    MaNhomHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhomHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomHang", x => x.MaNhomHang);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NgayDatHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHang_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaVach = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    MaNhomHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPham_NhomHang_MaNhomHang",
                        column: x => x.MaNhomHang,
                        principalTable: "NhomHang",
                        principalColumn: "MaNhomHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.MaChiTietDonHang);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KhachHang",
                columns: new[] { "MaKhachHang", "DiaChi", "Email", "HoTen", "SoDienThoai" },
                values: new object[,]
                {
                    { 1, "25, đường Nguyễn Văn Cừ, phường Nguyễn Cư Trinh, quận 1, Thành phố Hồ Chí Minh", "nguyenvanan@gmail.com", "Nguyễn Văn An", "0900000001" },
                    { 2, "18, đường Lê Lợi, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh", "tranthibinh@gmail.com", "Trần Thị Bình", "0900000002" },
                    { 3, "42, đường Phạm Văn Đồng, phường Hiệp Bình Chánh, thành phố Thủ Đức, Thành phố Hồ Chí Minh", "levancuong@gmail.com", "Lê Văn Cường", "0900000003" },
                    { 4, "76, đường Đồng Khởi, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh", "phamthidung@gmail.com", "Phạm Thị Dung", "0900000004" },
                    { 5, "12, đường Nguyễn Trãi, phường Bến Thành, quận 1, Thành phố Hồ Chí Minh", "hoangvanminh@gmail.com", "Hoàng Văn Minh", "0900000005" },
                    { 6, "35, đường Nguyễn Huệ, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh", "nguyenthilan@gmail.com", "Nguyễn Thị Lan", "0900000006" },
                    { 7, "89, đường Xô Viết Nghệ Tĩnh, phường 21, quận Bình Thạnh, Thành phố Hồ Chí Minh", "tranvanhung@gmail.com", "Trần Văn Hùng", "0900000007" },
                    { 8, "56, đường Võ Văn Ngân, phường Linh Chiểu, thành phố Thủ Đức, Thành phố Hồ Chí Minh", "lethimai@gmail.com", "Lê Thị Mai", "0900000008" },
                    { 9, "23, đường Nguyễn Văn Linh, phường Tân Phong, quận 7, Thành phố Hồ Chí Minh", "phamvannam@gmail.com", "Phạm Văn Nam", "0900000009" },
                    { 10, "101, đường Cách Mạng Tháng Tám, phường 7, quận 3, Thành phố Hồ Chí Minh", "hoangthihoa@gmail.com", "Hoàng Thị Hoa", "0900000010" },
                    { 11, "64, đường Lê Văn Việt, phường Tăng Nhơn Phú A, thành phố Thủ Đức, Thành phố Hồ Chí Minh", "dovanthanh@gmail.com", "Đỗ Văn Thành", "0900000011" },
                    { 12, "28, đường Hoàng Văn Thụ, phường 4, quận Tân Bình, Thành phố Hồ Chí Minh", "vuthihuong@gmail.com", "Vũ Thị Hương", "0900000012" },
                    { 13, "45, đường Trần Hưng Đạo, phường Phú Cường, thành phố Thủ Dầu Một, tỉnh Bình Dương", "nguyenvanlong@gmail.com", "Nguyễn Văn Long", "0900000013" },
                    { 14, "73, đường Điện Biên Phủ, phường Đa Kao, quận 1, Thành phố Hồ Chí Minh", "tranthingoc@gmail.com", "Trần Thị Ngọc", "0900000014" },
                    { 15, "19, đường Nguyễn Tất Thành, phường Phước Nguyên, thành phố Bà Rịa, tỉnh Bà Rịa - Vũng Tàu", "levanphuc@gmail.com", "Lê Văn Phúc", "0900000015" }
                });

            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "MaNguoiDung", "DangHoatDong", "HoTen", "MatKhau", "TenDangNhap", "VaiTro" },
                values: new object[,]
                {
                    { 1, true, "Quản trị viên", "123456", "admin", "Admin" },
                    { 2, true, "Nguyễn Văn Thuận", "123456", "cashier1", "Cashier" },
                    { 3, true, "Trần Văn Nam", "123456", "cashier2", "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "NhomHang",
                columns: new[] { "MaNhomHang", "MoTa", "TenNhomHang" },
                values: new object[,]
                {
                    { 1, "Các sản phẩm thực phẩm hữu cơ, tự nhiên", "Thực phẩm hữu cơ" },
                    { 2, "Nước uống, trà và các sản phẩm có nguồn gốc tự nhiên", "Đồ uống thiên nhiên" },
                    { 3, "Các sản phẩm chăm sóc cá nhân thân thiện với môi trường", "Sản phẩm chăm sóc cá nhân" },
                    { 4, "Đồ dùng gia đình có thể tái sử dụng", "Đồ dùng gia đình xanh" },
                    { 5, "Các sản phẩm được làm từ vật liệu tái chế", "Sản phẩm tái chế" },
                    { 6, "Các loại rau củ được trồng theo phương pháp hữu cơ", "Rau củ hữu cơ" },
                    { 7, "Các loại trái cây sạch và tự nhiên", "Trái cây hữu cơ" },
                    { 8, "Các loại ngũ cốc và hạt dinh dưỡng", "Ngũ cốc" },
                    { 9, "Các loại thực phẩm khô tiện dụng", "Thực phẩm khô" },
                    { 10, "Các loại gia vị có nguồn gốc tự nhiên", "Gia vị tự nhiên" },
                    { 11, "Các sản phẩm sử dụng trong nhà bếp", "Đồ dùng nhà bếp" },
                    { 12, "Các sản phẩm phục vụ nhu cầu cá nhân", "Đồ dùng cá nhân" },
                    { 13, "Văn phòng phẩm thân thiện với môi trường", "Văn phòng phẩm xanh" },
                    { 14, "Các sản phẩm vệ sinh an toàn", "Sản phẩm vệ sinh" },
                    { 15, "Các sản phẩm góp phần bảo vệ môi trường", "Sản phẩm thân thiện môi trường" }
                });

            migrationBuilder.InsertData(
                table: "DonHang",
                columns: new[] { "MaDonHang", "MaKhachHang", "MaNguoiDung", "NgayDatHang", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), 205000m, "Completed" },
                    { 2, 2, 2, new DateTime(2026, 10, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 115000m, "Completed" },
                    { 3, 3, 2, new DateTime(2026, 10, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), 165000m, "Completed" },
                    { 4, 4, 3, new DateTime(2026, 10, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), 95000m, "Completed" },
                    { 5, 5, 1, new DateTime(2026, 10, 3, 9, 30, 0, 0, DateTimeKind.Unspecified), 110000m, "Completed" },
                    { 6, 1, 2, new DateTime(2026, 10, 3, 13, 0, 0, 0, DateTimeKind.Unspecified), 88000m, "Completed" },
                    { 7, 2, 3, new DateTime(2026, 10, 4, 10, 30, 0, 0, DateTimeKind.Unspecified), 170000m, "Completed" },
                    { 8, 3, 1, new DateTime(2026, 10, 5, 15, 0, 0, 0, DateTimeKind.Unspecified), 165000m, "Completed" },
                    { 9, 4, 2, new DateTime(2026, 10, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), 185000m, "Completed" },
                    { 10, 5, 3, new DateTime(2026, 10, 7, 17, 0, 0, 0, DateTimeKind.Unspecified), 195000m, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "MaSanPham", "GiaBan", "MaNhomHang", "MaVach", "SoLuongTon", "TenSanPham" },
                values: new object[,]
                {
                    { 1, 85000m, 1, "893000000001", 50, "Gạo lứt hữu cơ" },
                    { 2, 120000m, 1, "893000000002", 30, "Mật ong nguyên chất" },
                    { 3, 35000m, 2, "893000000003", 60, "Nước ép cam nguyên chất" },
                    { 4, 45000m, 2, "893000000004", 45, "Trà xanh thiên nhiên" },
                    { 5, 55000m, 3, "893000000005", 35, "Xà phòng thiên nhiên" },
                    { 6, 110000m, 3, "893000000006", 25, "Dầu gội thảo mộc" },
                    { 7, 45000m, 4, "893000000007", 70, "Túi vải tái sử dụng" },
                    { 8, 25000m, 4, "893000000008", 80, "Ống hút tre" },
                    { 9, 35000m, 5, "893000000009", 50, "Sổ tay giấy tái chế" },
                    { 10, 75000m, 5, "893000000010", 25, "Chậu cây nhựa tái chế" },
                    { 11, 30000m, 6, "893000000011", 40, "Cải xanh hữu cơ" },
                    { 12, 28000m, 6, "893000000012", 45, "Cà rốt hữu cơ" },
                    { 13, 65000m, 7, "893000000013", 35, "Táo hữu cơ" },
                    { 14, 40000m, 7, "893000000014", 50, "Chuối hữu cơ" },
                    { 15, 90000m, 8, "893000000015", 30, "Yến mạch nguyên hạt" },
                    { 16, 75000m, 8, "893000000016", 35, "Hạt chia" },
                    { 17, 95000m, 9, "893000000017", 25, "Nấm hương khô" },
                    { 18, 70000m, 9, "893000000018", 30, "Mộc nhĩ khô" },
                    { 19, 20000m, 10, "893000000019", 60, "Muối biển tự nhiên" },
                    { 20, 45000m, 10, "893000000020", 50, "Tiêu đen hữu cơ" },
                    { 21, 95000m, 11, "893000000021", 30, "Hộp đựng thực phẩm thủy tinh" },
                    { 22, 25000m, 11, "893000000022", 70, "Muỗng tre" },
                    { 23, 40000m, 12, "893000000023", 45, "Khăn tay cotton" },
                    { 24, 85000m, 12, "893000000024", 40, "Bình nước cá nhân" },
                    { 25, 35000m, 13, "893000000025", 50, "Sổ tay tái chế" },
                    { 26, 15000m, 13, "893000000026", 80, "Bút thân thiện môi trường" },
                    { 27, 65000m, 14, "893000000027", 35, "Nước lau sàn sinh học" },
                    { 28, 60000m, 14, "893000000028", 40, "Nước rửa chén sinh học" },
                    { 29, 15000m, 15, "893000000029", 100, "Túi giấy thân thiện môi trường" },
                    { 30, 180000m, 15, "893000000030", 25, "Bình giữ nhiệt inox" }
                });

            migrationBuilder.InsertData(
                table: "ChiTietDonHang",
                columns: new[] { "MaChiTietDonHang", "DonGia", "MaDonHang", "MaSanPham", "SoLuong", "ThanhTien" },
                values: new object[,]
                {
                    { 1, 85000m, 1, 1, 1, 85000m },
                    { 2, 120000m, 1, 2, 1, 120000m },
                    { 3, 35000m, 2, 3, 2, 70000m },
                    { 4, 45000m, 2, 4, 1, 45000m },
                    { 5, 55000m, 3, 5, 1, 55000m },
                    { 6, 110000m, 3, 6, 1, 110000m },
                    { 7, 45000m, 4, 7, 1, 45000m },
                    { 8, 25000m, 4, 8, 2, 50000m },
                    { 9, 35000m, 5, 9, 1, 35000m },
                    { 10, 75000m, 5, 10, 1, 75000m },
                    { 11, 30000m, 6, 11, 2, 60000m },
                    { 12, 28000m, 6, 12, 1, 28000m },
                    { 13, 65000m, 7, 13, 2, 130000m },
                    { 14, 40000m, 7, 14, 1, 40000m },
                    { 15, 90000m, 8, 15, 1, 90000m },
                    { 16, 75000m, 8, 16, 1, 75000m },
                    { 17, 45000m, 9, 19, 2, 90000m },
                    { 18, 95000m, 9, 21, 1, 95000m },
                    { 19, 15000m, 10, 29, 1, 15000m },
                    { 20, 180000m, 10, 30, 1, 180000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaSanPham",
                table: "ChiTietDonHang",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaKhachHang",
                table: "DonHang",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaNhomHang",
                table: "SanPham",
                column: "MaNhomHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "NhomHang");
        }
    }
}
