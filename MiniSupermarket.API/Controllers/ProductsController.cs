
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    // API quản lý sản phẩm
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        // Khởi tạo Controller
        public ProductsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // LINQ được thực thi trên SQL Server: JOIN nhóm hàng, ORDER BY, OFFSET/FETCH.
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<ActionResult<PagedResult<ProductItemDto>>> GetPagedProducts(
            [FromQuery] string? keyword, [FromQuery] int? categoryId,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            pageNumber = Math.Max(1, pageNumber);
            if (pageSize < 1 || pageSize > 50) pageSize = 5;

            var query = _context.SanPhams.Include(p => p.NhomHang)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim().ToLower();
                query = query.Where(p => p.TenSanPham.ToLower().Contains(search)
                    || p.MaVach.Contains(search));
            }
            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(p => p.MaNhomHang == categoryId);

            int totalItems = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            // Tránh tràn số Skip khi client gửi số trang quá lớn.
            long offset = (long)(pageNumber - 1) * pageSize;
            var items = offset > int.MaxValue ? new List<ProductItemDto>() : await query
                .OrderBy(p => p.MaSanPham).Skip((int)offset).Take(pageSize)
                .Select(p => new ProductItemDto
                {
                    ProductId = p.MaSanPham, Barcode = p.MaVach,
                    ProductName = p.TenSanPham, Price = p.GiaBan,
                    StockQuantity = p.SoLuongTon, CategoryId = p.MaNhomHang,
                    CategoryName = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                }).ToListAsync(cancellationToken);

            return Ok(new PagedResult<ProductItemDto>
            {
                CurrentPage = pageNumber, PageSize = pageSize, TotalItems = totalItems,
                TotalPages = totalPages, Items = items
            });
        }

        // Lấy danh sách toàn bộ sản phẩm
        [HttpGet]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.SanPhams
                .AsNoTracking()
                .Include(p => p.NhomHang)
                .Select(p => new
                {
                    p.MaSanPham,
                    p.MaVach,
                    p.TenSanPham,
                    p.GiaBan,
                    p.SoLuongTon,
                    p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null
                        ? p.NhomHang.TenNhomHang
                        : ""
                })
                .ToListAsync();

            return Ok(products);
        }

        // Lấy thông tin một sản phẩm theo mã
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.SanPhams
                .AsNoTracking()
                .Where(p => p.MaSanPham == id)
                .Select(p => new
                {
                    p.MaSanPham,
                    p.MaVach,
                    p.TenSanPham,
                    p.GiaBan,
                    p.SoLuongTon,
                    p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null
                        ? p.NhomHang.TenNhomHang
                        : ""
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm.");

            return Ok(product);
        }

        // Tra cứu nhanh sản phẩm bằng máy quét mã vạch tại quầy POS.
        [HttpGet("barcode/{barcode}")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            barcode = (barcode ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(barcode))
                return BadRequest("Mã vạch không được để trống.");

            var product = await _context.SanPhams
                .AsNoTracking()
                .Where(p => p.MaVach == barcode)
                .Select(p => new
                {
                    p.MaSanPham,
                    p.MaVach,
                    p.TenSanPham,
                    p.GiaBan,
                    p.SoLuongTon,
                    p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null
                        ? p.NhomHang.TenNhomHang
                        : string.Empty
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm có mã vạch này.");

            return Ok(product);
        }

        // Tìm kiếm sản phẩm theo tên hoặc mã vạch
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Vui lòng nhập từ khóa.");

            var products = await _context.SanPhams
                .AsNoTracking()
                .Where(p =>
                    p.TenSanPham.Contains(keyword) ||
                    p.MaVach.Contains(keyword))
                .Select(p => new
                {
                    p.MaSanPham,
                    p.MaVach,
                    p.TenSanPham,
                    p.GiaBan,
                    p.SoLuongTon,
                    p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null
                        ? p.NhomHang.TenNhomHang
                        : ""
                })
                .ToListAsync();

            return Ok(products);
        }

        // Thêm sản phẩm mới
        [HttpPost]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Create(
            [FromBody] SanPham product)
        {
            if (string.IsNullOrWhiteSpace(product.TenSanPham))
                return BadRequest("Tên sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(product.MaVach))
                return BadRequest("Mã vạch không được để trống.");

            if (product.GiaBan < 0 || product.SoLuongTon < 0)
                return BadRequest("Giá bán và tồn kho không được âm.");

            bool categoryExists = await _context.NhomHangs
                .AnyAsync(c => c.MaNhomHang == product.MaNhomHang);

            if (!categoryExists)
                return BadRequest("Nhóm hàng không tồn tại.");

            bool barcodeExists = await _context.SanPhams
                .AnyAsync(p => p.MaVach == product.MaVach);

            if (barcodeExists)
                return Conflict("Mã vạch đã tồn tại.");

            product.MaSanPham = 0;
            product.NhomHang = null!;

            _context.SanPhams.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // Cập nhật thông tin sản phẩm
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] SanPham product)
        {
            var existing = await _context.SanPhams.FindAsync(id);

            if (existing == null)
                return NotFound("Không tìm thấy sản phẩm.");

            if (string.IsNullOrWhiteSpace(product.TenSanPham) ||
                string.IsNullOrWhiteSpace(product.MaVach))
                return BadRequest("Tên sản phẩm và mã vạch không được trống.");

            if (product.GiaBan < 0 || product.SoLuongTon < 0)
                return BadRequest("Giá bán và tồn kho không được âm.");

            bool categoryExists = await _context.NhomHangs
                .AnyAsync(c => c.MaNhomHang == product.MaNhomHang);

            if (!categoryExists)
                return BadRequest("Nhóm hàng không tồn tại.");

            bool barcodeExists = await _context.SanPhams
                .AnyAsync(p =>
                    p.MaVach == product.MaVach &&
                    p.MaSanPham != id);

            if (barcodeExists)
                return Conflict("Mã vạch đã tồn tại.");

            existing.MaVach = product.MaVach;
            existing.TenSanPham = product.TenSanPham;
            existing.GiaBan = product.GiaBan;
            existing.SoLuongTon = product.SoLuongTon;
            existing.MaNhomHang = product.MaNhomHang;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // Xóa sản phẩm
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.SanPhams.FindAsync(id);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm.");

            bool usedInOrders = await _context.ChiTietDonHangs
                .AnyAsync(d => d.MaSanPham == id);

            if (usedInOrders)
                return Conflict(
                    "Sản phẩm đã có trong đơn hàng, không thể xóa.");

            _context.SanPhams.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa sản phẩm thành công."
            });
        }
    }
}
