using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public ReportsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET /api/reports/quick?date=2026-10-01
        [HttpGet("quick")]
        public async Task<IActionResult> GetQuickReport(
            [FromQuery] DateTime? date)
        {
            DateTime reportDate = (date ?? DateTime.Today).Date;
            DateTime nextDate = reportDate.AddDays(1);

            var completedOrders = _context.DonHangs
                .AsNoTracking()
                .Where(x =>
                    x.TrangThai == "Completed" &&
                    x.NgayDatHang >= reportDate &&
                    x.NgayDatHang < nextDate);

            int totalOrders = await completedOrders.CountAsync();

            decimal totalRevenue = await completedOrders
                .Select(x => (decimal?)x.TongTien)
                .SumAsync() ?? 0;

            var bestSeller = await _context.ChiTietDonHangs
                .AsNoTracking()
                .Where(x =>
                    x.DonHang != null &&
                    x.DonHang.TrangThai == "Completed" &&
                    x.DonHang.NgayDatHang >= reportDate &&
                    x.DonHang.NgayDatHang < nextDate)
                .GroupBy(x => new
                {
                    x.MaSanPham,
                    TenSanPham = x.SanPham != null
                        ? x.SanPham.TenSanPham
                        : ""
                })
                .Select(group => new
                {
                    group.Key.MaSanPham,
                    group.Key.TenSanPham,
                    SoLuongDaBan = group.Sum(x => x.SoLuong)
                })
                .OrderByDescending(x => x.SoLuongDaBan)
                .ThenBy(x => x.TenSanPham)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                NgayBaoCao = reportDate,
                TongSoHoaDon = totalOrders,
                TongDoanhThu = totalRevenue,
                MatHangBanChayNhat = bestSeller?.TenSanPham
                    ?? "Chưa có dữ liệu",
                SoLuongMatHangBanChay = bestSeller?.SoLuongDaBan ?? 0
            });
        }
    }
}
