
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.Security.Claims;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Cashier")]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // =============================================
        // 1. LẤY DANH SÁCH ĐƠN HÀNG
        // GET /api/orders
        // =============================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.DonHangs
                .AsNoTracking()
                .OrderByDescending(x => x.NgayDatHang)
                .Select(x => new
                {
                    x.MaDonHang,
                    x.MaKhachHang,
                    TenKhachHang = x.KhachHang != null
                        ? x.KhachHang.HoTen : "",
                    x.MaNguoiDung,
                    NhanVien = x.NguoiDung != null
                        ? x.NguoiDung.TenDangNhap : "",
                    x.NgayDatHang,
                    x.TongTien,
                    x.TrangThai
                })
                .ToListAsync();

            return Ok(orders);
        }

        // =============================================
        // 2. LẤY CHI TIẾT ĐƠN HÀNG
        // GET /api/orders/1
        // =============================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.DonHangs
                .AsNoTracking()
                .Where(x => x.MaDonHang == id)
                .Select(x => new
                {
                    x.MaDonHang,
                    x.MaKhachHang,
                    TenKhachHang = x.KhachHang != null
                        ? x.KhachHang.HoTen : "",
                    x.MaNguoiDung,
                    NhanVien = x.NguoiDung != null
                        ? x.NguoiDung.TenDangNhap : "",
                    x.NgayDatHang,
                    x.TongTien,
                    x.TrangThai,

                    ChiTiet = _context.ChiTietDonHangs
                        .Where(ct => ct.MaDonHang == x.MaDonHang)
                        .Select(ct => new
                        {
                            ct.MaChiTietDonHang,
                            ct.MaSanPham,
                            TenSanPham = ct.SanPham != null
                                ? ct.SanPham.TenSanPham : "",
                            ct.SoLuong,
                            ct.DonGia,
                            ct.ThanhTien
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy đơn hàng!"
                });
            }

            return Ok(order);
        }

        // =============================================
        // 3. THANH TOÁN - TẠO ĐƠN HÀNG
        // POST /api/orders/checkout
        // =============================================
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(
            [FromBody] CheckoutRequest request)
        {
            if (request == null ||
                request.SanPhams == null ||
                request.SanPhams.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Thông tin đơn hàng không hợp lệ!"
                });
            }

            // Kiểm tra số lượng đầu vào
            if (request.SanPhams.Any(x =>
                x.MaSanPham <= 0 || x.SoLuong <= 0))
            {
                return BadRequest(new
                {
                    message = "Mã sản phẩm hoặc số lượng không hợp lệ!"
                });
            }

            // Không cho trùng sản phẩm trong cùng yêu cầu
            if (request.SanPhams
                .GroupBy(x => x.MaSanPham)
                .Any(g => g.Count() > 1))
            {
                return BadRequest(new
                {
                    message = "Danh sách có sản phẩm bị trùng!"
                });
            }

            // Lấy tên đăng nhập từ JWT
            string? username =
                User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Không xác định được người đăng nhập!"
                });
            }

            // Xác định nhân viên lập đơn
            var employee = await _context.NguoiDungs
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.TenDangNhap == username &&
                    x.DangHoatDong);

            if (employee == null)
            {
                return Unauthorized(new
                {
                    message = "Tài khoản không tồn tại hoặc đã bị khóa!"
                });
            }

            // Khách hàng là tùy chọn để hỗ trợ khách vãng lai.
            int? customerId = null;

            if (request.MaKhachHang.HasValue &&
                request.MaKhachHang.Value > 0)
            {
                customerId = await _context.KhachHangs
                    .Where(x =>
                        x.MaKhachHang == request.MaKhachHang.Value)
                    .Select(x => (int?)x.MaKhachHang)
                    .FirstOrDefaultAsync();

                if (!customerId.HasValue)
                {
                    return BadRequest(new
                    {
                        message = "Khách hàng không tồn tại!"
                    });
                }
            }
            else if (!string.IsNullOrWhiteSpace(
                request.SoDienThoaiKhachHang))
            {
                string phone =
                    request.SoDienThoaiKhachHang.Trim();

                customerId = await _context.KhachHangs
                    .Where(x => x.SoDienThoai == phone)
                    .Select(x => (int?)x.MaKhachHang)
                    .FirstOrDefaultAsync();

                if (!customerId.HasValue)
                {
                    return BadRequest(new
                    {
                        message =
                            "Không tìm thấy khách hàng theo số điện thoại!"
                    });
                }
            }

            // Transaction đảm bảo lưu đơn và trừ tồn kho
            // cùng thành công hoặc cùng thất bại
            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync(
                        System.Data.IsolationLevel.Serializable);

            try
            {
                var order = new DonHang
                {
                    MaKhachHang = customerId,
                    MaNguoiDung = employee.MaNguoiDung,
                    NgayDatHang = DateTime.Now,
                    TongTien = 0,
                    TrangThai = "Completed"
                };

                decimal total = 0;

                var details = new List<ChiTietDonHang>();

                foreach (var item in request.SanPhams)
                {
                    // Tìm sản phẩm trong SQL Server
                    var product = await _context.SanPhams
                        .FirstOrDefaultAsync(x =>
                            x.MaSanPham == item.MaSanPham);

                    if (product == null)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message =
                                $"Không tìm thấy sản phẩm mã {item.MaSanPham}!"
                        });
                    }

                    // Kiểm tra tồn kho
                    if (product.SoLuongTon < item.SoLuong)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message =
                                $"Sản phẩm '{product.TenSanPham}' " +
                                $"chỉ còn {product.SoLuongTon} trong kho!"
                        });
                    }

                    // Kiểm tra giá bán
                    if (product.GiaBan < 0)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message =
                                $"Giá bán sản phẩm '{product.TenSanPham}' không hợp lệ!"
                        });
                    }

                    // Lấy giá trực tiếp từ cơ sở dữ liệu
                    decimal unitPrice = product.GiaBan;
                    decimal lineTotal =
                        unitPrice * item.SoLuong;

                    total += lineTotal;

                    // Trừ số lượng tồn
                    product.SoLuongTon -= item.SoLuong;

                    // Tạo chi tiết đơn hàng
                    details.Add(new ChiTietDonHang
                    {
                        MaSanPham = product.MaSanPham,
                        SoLuong = item.SoLuong,
                        DonGia = unitPrice,
                        ThanhTien = lineTotal
                    });
                }

                order.TongTien = total;
                order.ChiTietDonHangs = details;

                // Lưu đơn hàng và chi tiết
                _context.DonHangs.Add(order);

                await _context.SaveChangesAsync();

                // Hoàn tất transaction
                await transaction.CommitAsync();

                return Ok(new
                {
                    success = true,
                    message = "Thanh toán thành công!",
                    maDonHang = order.MaDonHang,
                    tongTien = order.TongTien,
                    ngayDatHang = order.NgayDatHang,
                    trangThai = order.TrangThai
                });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    success = false,
                    message = "Thanh toán thất bại. Dữ liệu chưa được lưu."
                });
            }
        }
    }

    // =============================================
    // DTO THANH TOÁN
    // =============================================
    public class CheckoutRequest
    {
        public int? MaKhachHang { get; set; }

        public string? SoDienThoaiKhachHang { get; set; }

        public List<CheckoutProductRequest> SanPhams
        { get; set; } = new();
    }

    public class CheckoutProductRequest
    {
        public int MaSanPham { get; set; }

        public int SoLuong { get; set; }
    }
}
