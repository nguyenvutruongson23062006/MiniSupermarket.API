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
            var list = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // ============================================================
        // 2. READ: Lấy chi tiết một nhóm hàng theo ID
        // GET /api/categories/{id}
        // Admin và Cashier đều được phép xem
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _context.Categories.FindAsync(id);

            if (cat == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng!"
                });
            }

            return Ok(cat);
        }

        // ============================================================
        // 3. SEARCH: Tìm kiếm nhóm hàng
        // GET /api/categories/search?keyword=...
        // Admin và Cashier đều được phép tìm kiếm
        // ============================================================
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new
                {
                    message = "Vui lòng nhập từ khóa!"
                });
            }

            var result = await _context.Categories
                .Where(c => c.CategoryName.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }

        // ============================================================
        // 4. CREATE: Thêm mới nhóm hàng
        // POST /api/categories
        // CHỈ ADMIN được phép thêm
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Category newCat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Thêm nhóm hàng vào DbContext
            // CategoryId sẽ được SQL Server tự động tăng
            _context.Categories.Add(newCat);

            // Lưu thay đổi vào SQL Server
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newCat.CategoryId },
                newCat);
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
            [FromBody] Category updateCat)
        {
            var cat = await _context.Categories.FindAsync(id);

            if (cat == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần sửa!"
                });
            }

            // Cập nhật thông tin nhóm hàng
            cat.CategoryName = updateCat.CategoryName;
            cat.Description = updateCat.Description;

            // Lưu thay đổi vào SQL Server
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================================================
        // 6. DELETE: Xóa nhóm hàng theo ID
        // DELETE /api/categories/{id}
        // CHỈ ADMIN được phép xóa
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.Categories.FindAsync(id);

            if (cat == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần xóa!"
                });
            }

            // Xóa nhóm hàng khỏi DbContext
            _context.Categories.Remove(cat);

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