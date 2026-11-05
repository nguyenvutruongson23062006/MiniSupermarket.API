using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace MiniSupermarket.API.Controllers;

public partial class ReportsController
{
    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> DashboardSummary(CancellationToken ct)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var orders = _context.DonHangs.AsNoTracking().Where(x => x.TrangThai == "Completed");
        return Ok(new
        {
            RevenueToday = await orders.Where(x => x.NgayDatHang >= today && x.NgayDatHang < tomorrow)
                .SumAsync(x => (decimal?)x.TongTien, ct) ?? 0,
            TotalOrders = await orders.CountAsync(ct),
            NewCustomersToday = await _context.KhachHangs.AsNoTracking()
                .CountAsync(x => x.NgayDangKy >= today && x.NgayDangKy < tomorrow, ct),
            LowStockProducts = await _context.SanPhams.AsNoTracking().CountAsync(x => x.SoLuongTon <= 10, ct)
        });
    }

    private static bool ValidRange(DateTime fromDate, DateTime toDate) =>
        fromDate != default && toDate != default && fromDate.Date <= toDate.Date &&
        toDate.Date < DateTime.MaxValue.Date && (toDate.Date - fromDate.Date).TotalDays <= 366;

    [HttpGet("daily-revenue")]
    public async Task<IActionResult> DailyRevenue(DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        if (!ValidRange(fromDate, toDate)) return BadRequest(new { message = "Chọn khoảng ngày hợp lệ, tối đa 366 ngày." });
        await using var connection = new SqlConnection(_context.Database.GetConnectionString());
        using var command = new SqlCommand("dbo.sp_GetDailyRevenue", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate.Date;
        command.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate.Date;
        await connection.OpenAsync(ct);
        var totals = new Dictionary<DateTime, decimal>();
        await using (var reader = await command.ExecuteReaderAsync(ct))
            while (await reader.ReadAsync(ct)) totals[reader.GetDateTime(0).Date] = reader.GetDecimal(1);
        var rows = new List<DailyRevenueDto>();
        for (var day = fromDate.Date; day <= toDate.Date; day = day.AddDays(1))
            rows.Add(new(day, totals.GetValueOrDefault(day)));
        return Ok(rows);
    }

    [HttpGet("best-sellers")]
    public async Task<IActionResult> BestSellers(DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        if (!ValidRange(fromDate, toDate)) return BadRequest(new { message = "Chọn khoảng ngày hợp lệ, tối đa 366 ngày." });
        var end = toDate.Date.AddDays(1);
        var start = fromDate.Date;
        var data = await _context.ChiTietDonHangs.AsNoTracking()
            .Where(x => x.DonHang != null && x.DonHang.TrangThai == "Completed" &&
                x.DonHang.NgayDatHang >= start && x.DonHang.NgayDatHang < end)
            .GroupBy(x => new { x.MaSanPham, x.SanPham!.TenSanPham })
            .Select(g => new { ProductId = g.Key.MaSanPham, ProductName = g.Key.TenSanPham,
                TotalQuantity = g.Sum(x => x.SoLuong), GrossSales = g.Sum(x => x.ThanhTien) })
            .OrderByDescending(x => x.TotalQuantity).ThenBy(x => x.ProductId).Take(5).ToListAsync(ct);
        return Ok(data);
    }

    // CSV UTF-8 BOM để Excel hiển thị đúng tiếng Việt; chặn ô bắt đầu bằng công thức.
    private FileContentResult Csv(string name, IEnumerable<IEnumerable<object?>> rows)
    {
        static string Cell(object? value)
        {
            var text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
            if (value is string && text.TrimStart().StartsWithAnyFormula()) text = "'" + text;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        var csv = string.Join("\r\n", rows.Select(row => string.Join(",", row.Select(Cell))));
        return File(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray(), "text/csv; charset=utf-8", name);
    }

    [HttpGet("export-products")]
    public async Task<IActionResult> ExportProducts(CancellationToken ct)
    {
        var products = await _context.SanPhams.AsNoTracking().OrderBy(x => x.MaSanPham)
            .Select(x => new { x.MaSanPham, x.MaVach, x.TenSanPham, x.GiaBan, x.SoLuongTon,
                Category = x.NhomHang != null ? x.NhomHang.TenNhomHang : "" }).ToListAsync(ct);
        var rows = new List<IEnumerable<object?>> { new object[] { "Mã SP", "Mã vạch", "Tên sản phẩm", "Giá bán", "Tồn kho", "Nhóm hàng" } };
        rows.AddRange(products.Select(x => new object[] { x.MaSanPham, x.MaVach, x.TenSanPham, x.GiaBan, x.SoLuongTon, x.Category }));
        return Csv("DanhMucSanPham.csv", rows);
    }

    [HttpGet("export-orders")]
    public async Task<IActionResult> ExportOrders(DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        if (!ValidRange(fromDate, toDate)) return BadRequest(new { message = "Chọn khoảng ngày hợp lệ, tối đa 366 ngày." });
        var start = fromDate.Date; var end = toDate.Date.AddDays(1);
        var orders = await _context.DonHangs.AsNoTracking().Where(x => x.TrangThai == "Completed" &&
            x.NgayDatHang >= start && x.NgayDatHang < end).OrderBy(x => x.NgayDatHang).ThenBy(x => x.MaDonHang)
            .Select(x => new { x.MaDonHang, x.MaHoaDon, x.NgayDatHang, x.TongTien, x.DiemDaDung, x.TienGiamDiem,
                Cashier = x.NguoiDung != null ? x.NguoiDung.TenDangNhap : "" }).ToListAsync(ct);
        var rows = new List<IEnumerable<object?>> { new object[] { "Mã đơn", "Mã hóa đơn", "Ngày bán", "Thu ngân", "Điểm đã dùng", "Giảm từ điểm", "Thực thu" } };
        rows.AddRange(orders.Select(x => new object?[] { x.MaDonHang, x.MaHoaDon, x.NgayDatHang.ToString("yyyy-MM-dd HH:mm:ss"), x.Cashier, x.DiemDaDung, x.TienGiamDiem, x.TongTien }));
        return Csv("HoaDon.csv", rows);
    }
}

public record DailyRevenueDto(DateTime Date, decimal Revenue);
internal static class CsvTextExtensions
{
    public static bool StartsWithAnyFormula(this string text) => text.Length > 0 && "=+-@\t\r\n".Contains(text[0]);
}
