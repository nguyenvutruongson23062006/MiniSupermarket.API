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
        // 0. PAGING: Phân trang nhóm hàng trên máy chủ bằng LINQ Skip & Take
        // GET /api/categories/paged?keyword=...&pageNumber=1&pageSize=5
        // Admin và Warehouse được phép xem
        // ============================================================
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<ActionResult<PagedResult<CategoryDto>>> GetPagedCategories(
            [FromQuery] string? keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            pageNumber = Math.Max(1, pageNumber);
            if (pageSize < 1 || pageSize > 50) pageSize = 5;

            var query = _context.NhomHangs
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim().ToLower();
                query = query.Where(c => c.TenNhomHang.ToLower().Contains(search)
                    || (c.MoTa != null && c.MoTa.ToLower().Contains(search)));
            }

            int totalItems = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            long offset = (long)(pageNumber - 1) * pageSize;
            var items = offset > int.MaxValue ? new List<CategoryDto>() : await query
                .OrderBy(c => c.MaNhomHang)
                .Skip((int)offset)
                .Take(pageSize)
                .Select(c => new CategoryDto
                {
                    MaNhomHang = c.MaNhomHang,
                    TenNhomHang = c.TenNhomHang,
                    MoTa = c.MoTa
                })
                .ToListAsync(cancellationToken);

            return Ok(new PagedResult<CategoryDto>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            });
        }

        // ============================================================
        // 1. READ: Lấy toàn bộ danh sách nhóm hàng (Trả về DTO)
        // GET /api/categories
        // Admin và Warehouse được phép xem
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetAll()
        {
            var danhSach = await _context.NhomHangs
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    MaNhomHang = c.MaNhomHang,
                    TenNhomHang = c.TenNhomHang,
                    MoTa = c.MoTa
                })
                .ToListAsync();

            return Ok(danhSach);
        }

        // ============================================================
        // 2. READ: Lấy chi tiết một nhóm hàng theo mã
        // GET /api/categories/{id}
        // Admin và Warehouse được phép xem
        // ============================================================
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetById(int id)
        {
            var nhomHang = await _context.NhomHangs
                .AsNoTracking()
                .Where(c => c.MaNhomHang == id)
                .Select(c => new CategoryDto
                {
                    MaNhomHang = c.MaNhomHang,
                    TenNhomHang = c.TenNhomHang,
                    MoTa = c.MoTa
                })
                .FirstOrDefaultAsync();

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
        // 3. SEARCH: Tìm kiếm nhóm hàng theo từ khóa
        // GET /api/categories/search?keyword=...
        // Admin và Warehouse được phép tìm kiếm
        // ============================================================
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new
                {
                    message = "Vui lòng nhập từ khóa!"
                });
            }

            var ketQua = await _context.NhomHangs
                .AsNoTracking()
                .Where(nh => nh.TenNhomHang.Contains(keyword))
                .Select(c => new CategoryDto
                {
                    MaNhomHang = c.MaNhomHang,
                    TenNhomHang = c.TenNhomHang,
                    MoTa = c.MoTa
                })
                .ToListAsync();

            return Ok(ketQua);
        }

        // ============================================================
        // 4. CREATE: Thêm mới nhóm hàng (Nhận DTO, trả mã 201 Created)
        // POST /api/categories
        // Admin và Warehouse được phép thêm
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nhomHangMoi = new NhomHang
            {
                TenNhomHang = dto.TenNhomHang.Trim(),
                MoTa = dto.MoTa?.Trim()
            };

            // Thêm nhóm hàng vào DbContext
            _context.NhomHangs.Add(nhomHangMoi);
            await _context.SaveChangesAsync();

            var responseDto = new CategoryDto
            {
                MaNhomHang = nhomHangMoi.MaNhomHang,
                TenNhomHang = nhomHangMoi.TenNhomHang,
                MoTa = nhomHangMoi.MoTa
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.MaNhomHang },
                responseDto);
        }

        // ============================================================
        // 5. UPDATE: Cập nhật thông tin nhóm hàng (Nhận DTO)
        // PUT /api/categories/{id}
        // Admin và Warehouse được phép sửa
        // ============================================================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nhomHang = await _context.NhomHangs.FindAsync(id);

            if (nhomHang == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần sửa!"
                });
            }

            nhomHang.TenNhomHang = dto.TenNhomHang.Trim();
            nhomHang.MoTa = dto.MoTa?.Trim();

            await _context.SaveChangesAsync();

            return Ok(new CategoryDto
            {
                MaNhomHang = nhomHang.MaNhomHang,
                TenNhomHang = nhomHang.TenNhomHang,
                MoTa = nhomHang.MoTa
            });
        }

        // ============================================================
        // 6. DELETE: Xóa nhóm hàng theo mã
        // DELETE /api/categories/{id}
        // Admin và Warehouse được phép xóa
        // ============================================================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Delete(int id)
        {
            var nhomHang = await _context.NhomHangs.FindAsync(id);

            if (nhomHang == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy nhóm hàng cần xóa!"
                });
            }

            bool hasProducts = await _context.SanPhams.AnyAsync(p => p.MaNhomHang == id);
            if (hasProducts)
            {
                return BadRequest(new
                {
                    message = "Nhóm hàng đang có sản phẩm liên kết, không thể xóa!"
                });
            }

            _context.NhomHangs.Remove(nhomHang);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa nhóm hàng thành công!"
            });
        }

        // ============================================================
        // 7. KIỂM TRA QUYỀN ADMIN (Áp dụng async/await)
        // GET /api/categories/admin-dashboard
        // CHỈ ADMIN được phép truy cập
        // ============================================================
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            return await Task.FromResult<IActionResult>(Ok(new
            {
                message = "Chào mừng Admin! Bạn có toàn quyền quản trị hệ thống siêu thị mini."
            }));
        }

        // ============================================================
        // 8. KIỂM TRA QUYỀN CHUNG CHO NHÂN VIÊN (Áp dụng async/await)
        // GET /api/categories/staff-pos
        // ADMIN và CASHIER đều được phép truy cập
        // ============================================================
        [HttpGet("staff-pos")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> GetStaffPos()
        {
            return await Task.FromResult<IActionResult>(Ok(new
            {
                message = "Màn hình POS Thu ngân sẵn sàng phục vụ bán hàng."
            }));
        }
    }
}
