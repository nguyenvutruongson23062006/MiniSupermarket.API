using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        // DbContext dùng để kết nối và thao tác với SQL Server
        private readonly SupermarketDbContext _context;

        // Khởi tạo Controller và nhận DbContext từ Dependency Injection
        public CategoriesController(SupermarketDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // 1. READ: Lấy toàn bộ danh sách nhóm hàng
        // GET /api/categories
        // Admin và Cashier đều được phép xem
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var danhSach = await _context.NhomHangs
                .AsNoTracking()
                .ToListAsync();

            return Ok(danhSach);
        }

        // ============================================================
        // 2. READ: Lấy chi tiết một nhóm hàng theo mã
        // GET /api/categories/{id}
        // Admin và Cashier đều được phép xem
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nhomHang = await _context.NhomHangs
                .FindAsync(id);

            if (nhomHang == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng!"
                });
            }

            return Ok(nhomHang);
        }

        // ============================================================
        // 3. SEARCH: Tìm kiếm nhóm hàng
        // GET /api/categories/search?keyword=...
        // Admin và Cashier đều được phép tìm kiếm
        // ============================================================
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new
                {
                    message = "Vui lòng nhập từ khóa!"
                });
            }

            var ketQua = await _context.NhomHangs
                .Where(nh =>
                    nh.TenNhomHang.Contains(keyword))
                .AsNoTracking()
                .ToListAsync();

            return Ok(ketQua);
        }

        // ============================================================
        // 4. CREATE: Thêm mới nhóm hàng
        // POST /api/categories
        // CHỈ ADMIN được phép thêm
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] NhomHang nhomHangMoi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Thêm nhóm hàng vào DbContext
            // MaNhomHang sẽ được SQL Server tự động tăng
            _context.NhomHangs.Add(nhomHangMoi);

            // Lưu thay đổi vào SQL Server
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = nhomHangMoi.MaNhomHang
                },
                nhomHangMoi);
        }

        // ============================================================
        // 5. UPDATE: Cập nhật thông tin nhóm hàng
        // PUT /api/categories/{id}
        // CHỈ ADMIN được phép sửa
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] NhomHang nhomHangCapNhat)
        {
            var nhomHang = await _context.NhomHangs
                .FindAsync(id);

            if (nhomHang == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần sửa!"
                });
            }

            // Cập nhật tên nhóm hàng
            nhomHang.TenNhomHang =
                nhomHangCapNhat.TenNhomHang;

            // Cập nhật mô tả
            nhomHang.MoTa =
                nhomHangCapNhat.MoTa;

            // Lưu thay đổi vào SQL Server
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================================================
        // 6. DELETE: Xóa nhóm hàng theo mã
        // DELETE /api/categories/{id}
        // CHỈ ADMIN được phép xóa
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var nhomHang = await _context.NhomHangs
                .FindAsync(id);

            if (nhomHang == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần xóa!"
                });
            }

            // Xóa nhóm hàng khỏi DbContext
            _context.NhomHangs.Remove(nhomHang);

            // Lưu thay đổi vào SQL Server
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================================================
        // 7. KIỂM TRA QUYỀN ADMIN
        // GET /api/categories/admin-dashboard
        // CHỈ ADMIN được phép truy cập
        // ============================================================
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(new
            {
                message =
                    "Chào mừng Admin! Bạn có toàn quyền quản trị hệ thống siêu thị mini."
            });
        }

        // ============================================================
        // 8. KIỂM TRA QUYỀN CHUNG CHO NHÂN VIÊN
        // GET /api/categories/staff-pos
        // ADMIN và CASHIER đều được phép truy cập
        // ============================================================
        [HttpGet("staff-pos")]
        [Authorize(Roles = "Admin,Cashier")]
        public IActionResult GetStaffPos()
        {
            return Ok(new
            {
                message =
                    "Màn hình POS Thu ngân sẵn sàng phục vụ bán hàng."
            });
        }
    }
}