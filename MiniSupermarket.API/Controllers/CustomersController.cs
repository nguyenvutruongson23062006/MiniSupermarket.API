
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Cashier")]
    public class CustomersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CustomersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // Bài 6.1: điểm thưởng giảm dần, mã khách tăng dần để phân trang ổn định.
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<CustomerItemDto>>> GetPagedCustomers(
            [FromQuery] string? keyword, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            pageNumber = Math.Max(1, pageNumber);
            if (pageSize < 1 || pageSize > 50) pageSize = 5;
            var query = _context.KhachHangs.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim().ToLower();
                query = query.Where(c => c.HoTen.ToLower().Contains(search)
                    || c.SoDienThoai.Contains(search));
            }
            int totalItems = await query.CountAsync(cancellationToken);
            long offset = (long)(pageNumber - 1) * pageSize;
            var items = offset > int.MaxValue ? new List<CustomerItemDto>() : await query
                .OrderByDescending(c => c.DiemTichLuy).ThenBy(c => c.MaKhachHang)
                .Skip((int)offset).Take(pageSize)
                .Select(c => new CustomerItemDto
                {
                    CustomerId = c.MaKhachHang, FullName = c.HoTen,
                    Phone = c.SoDienThoai, Email = c.Email, Address = c.DiaChi,
                    RewardPoints = c.DiemTichLuy
                }).ToListAsync(cancellationToken);
            return Ok(new PagedResult<CustomerItemDto>
            {
                CurrentPage = pageNumber, PageSize = pageSize, TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize), Items = items
            });
        }

        // GET: api/customers
        // Lấy danh sách khách hàng
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _context.KhachHangs
                .AsNoTracking()
                .OrderBy(x => x.MaKhachHang)
                .Select(x => new
                {
                    x.MaKhachHang,
                    x.HoTen,
                    x.SoDienThoai,
                    x.Email,
                    x.DiaChi
                })
                .ToListAsync();

            return Ok(customers);
        }

        // GET: api/customers/1
        // Lấy khách hàng theo mã
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.KhachHangs
                .AsNoTracking()
                .Where(x => x.MaKhachHang == id)
                .Select(x => new
                {
                    x.MaKhachHang,
                    x.HoTen,
                    x.SoDienThoai,
                    x.Email,
                    x.DiaChi
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound("Không tìm thấy khách hàng.");

            return Ok(customer);
        }

        // GET: api/customers/search?keyword=...
        // Tìm theo tên, số điện thoại hoặc email
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Vui lòng nhập từ khóa.");

            keyword = keyword.Trim();

            var customers = await _context.KhachHangs
                .AsNoTracking()
                .Where(x =>
                    x.HoTen.Contains(keyword) ||
                    x.SoDienThoai.Contains(keyword) ||
                    (x.Email != null &&
                     x.Email.Contains(keyword)))
                .OrderBy(x => x.MaKhachHang)
                .Select(x => new
                {
                    x.MaKhachHang,
                    x.HoTen,
                    x.SoDienThoai,
                    x.Email,
                    x.DiaChi
                })
                .ToListAsync();

            return Ok(customers);
        }

        // POST: api/customers
        // Thêm khách hàng
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            string phone = request.SoDienThoai.Trim();

            bool phoneExists = await _context.KhachHangs
                .AnyAsync(x => x.SoDienThoai == phone);

            if (phoneExists)
                return Conflict("Số điện thoại khách hàng đã tồn tại.");

            var customer = new KhachHang
            {
                HoTen = request.HoTen.Trim(),
                SoDienThoai = phone,
                Email = string.IsNullOrWhiteSpace(request.Email)
                    ? null
                    : request.Email.Trim(),
                DiaChi = request.DiaChi.Trim()
            };

            _context.KhachHangs.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = customer.MaKhachHang },
                new
                {
                    customer.MaKhachHang,
                    customer.HoTen,
                    customer.SoDienThoai,
                    customer.Email,
                    customer.DiaChi
                });
        }

        // PUT: api/customers/1
        // Cập nhật khách hàng
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var customer = await _context.KhachHangs
                .FindAsync(id);

            if (customer == null)
                return NotFound("Không tìm thấy khách hàng.");

            string phone = request.SoDienThoai.Trim();

            bool phoneExists = await _context.KhachHangs
                .AnyAsync(x =>
                    x.SoDienThoai == phone &&
                    x.MaKhachHang != id);

            if (phoneExists)
                return Conflict("Số điện thoại khách hàng đã tồn tại.");

            customer.HoTen = request.HoTen.Trim();
            customer.SoDienThoai = phone;
            customer.Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();
            customer.DiaChi = request.DiaChi.Trim();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Cập nhật khách hàng thành công.",
                customer.MaKhachHang
            });
        }

        // DELETE: api/customers/1
        // Chỉ Admin được xóa khách hàng
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.KhachHangs
                .FindAsync(id);

            if (customer == null)
                return NotFound("Không tìm thấy khách hàng.");

            bool hasOrders = await _context.DonHangs
                .AnyAsync(x => x.MaKhachHang == id);

            if (hasOrders)
            {
                return Conflict(
                    "Khách hàng đã có đơn hàng, không thể xóa.");
            }

            _context.KhachHangs.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Xóa khách hàng thành công."
            });
        }
    }

    // DTO nhận dữ liệu từ WinForms
    public class CustomerRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [StringLength(500)]
        public string DiaChi { get; set; } = string.Empty;
    }
}
