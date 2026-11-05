using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace MiniSupermarket.API.Migrations;
public partial class CompleteDashboardAndLoyalty : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(name: "NgayDangKy", table: "KhachHang", type: "datetime2", nullable: true);
        migrationBuilder.AddColumn<int>(name: "DiemDaDung", table: "DonHang", type: "int", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<decimal>(name: "TienGiamDiem", table: "DonHang", type: "decimal(18,2)", nullable: false, defaultValue: 0m);
        migrationBuilder.Sql("""
            EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_GetDailyRevenue
                @FromDate date, @ToDate date
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT CAST(NgayDatHang AS date) AS SaleDate, SUM(TongTien) AS Revenue
                FROM dbo.DonHang
                WHERE TrangThai=N''Completed'' AND NgayDatHang>=@FromDate
                    AND NgayDatHang<DATEADD(day, 1, @ToDate)
                GROUP BY CAST(NgayDatHang AS date) ORDER BY SaleDate;
            END');
            """);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_GetDailyRevenue");
        migrationBuilder.DropColumn(name: "NgayDangKy", table: "KhachHang");
        migrationBuilder.DropColumn(name: "DiemDaDung", table: "DonHang");
        migrationBuilder.DropColumn(name: "TienGiamDiem", table: "DonHang");
    }
}
