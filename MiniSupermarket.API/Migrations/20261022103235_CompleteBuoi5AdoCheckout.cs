using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class CompleteBuoi5AdoCheckout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiemTichLuy",
                table: "KhachHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiemThuong",
                table: "DonHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaHoaDon",
                table: "DonHang",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TongSoLuong",
                table: "DonHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_GetRevenueByCashier
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT u.TenDangNhap AS CashierUsername, COUNT(*) AS TotalOrders,
                           SUM(d.TongTien) AS TotalRevenue, SUM(d.DiemThuong) AS TotalPointsAwarded
                    FROM dbo.DonHang d JOIN dbo.NguoiDung u ON u.MaNguoiDung=d.MaNguoiDung
                    WHERE d.TrangThai=N''Completed''
                    GROUP BY u.TenDangNhap ORDER BY TotalRevenue DESC, CashierUsername;
                END');
                """);
            migrationBuilder.Sql("""
                EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_GetRevenueByDateRange
                    @FromDate datetime2, @ToDate datetime2
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT u.TenDangNhap AS CashierUsername, COUNT(*) AS TotalOrders,
                           SUM(d.TongTien) AS TotalRevenue, SUM(d.DiemThuong) AS TotalPointsAwarded
                    FROM dbo.DonHang d JOIN dbo.NguoiDung u ON u.MaNguoiDung=d.MaNguoiDung
                    WHERE d.TrangThai=N''Completed''
                      AND d.NgayDatHang>=@FromDate AND d.NgayDatHang<DATEADD(day,1,@ToDate)
                    GROUP BY u.TenDangNhap ORDER BY TotalRevenue DESC, CashierUsername;
                END');
                """);
            migrationBuilder.Sql("""
                EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_GetTop5SellingProducts
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT TOP (5) p.MaSanPham AS ProductId, p.TenSanPham AS ProductName,
                           SUM(ct.SoLuong) AS TotalQuantity, SUM(ct.ThanhTien) AS TotalRevenue
                    FROM dbo.ChiTietDonHang ct
                    JOIN dbo.DonHang d ON d.MaDonHang=ct.MaDonHang
                    JOIN dbo.SanPham p ON p.MaSanPham=ct.MaSanPham
                    WHERE d.TrangThai=N''Completed''
                    GROUP BY p.MaSanPham,p.TenSanPham
                    ORDER BY TotalQuantity DESC,p.MaSanPham;
                END');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_GetRevenueByCashier");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_GetRevenueByDateRange");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_GetTop5SellingProducts");
            migrationBuilder.DropColumn(
                name: "DiemTichLuy",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "DiemThuong",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "MaHoaDon",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "TongSoLuong",
                table: "DonHang");
        }
    }
}
