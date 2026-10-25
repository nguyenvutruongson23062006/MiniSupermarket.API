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

        // ============================================================
        // 1. Phân trang dữ liệu sản phẩm trên máy chủ bằng LINQ Skip & Take
        // GET /api/products/paged?keyword=...&categoryId=...&pageNumber=1&pageSize=5
        // ============================================================
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<ActionResult<PagedResult<ProductItemDto>>> GetPagedProducts(
            [FromQuery] string? keyword, [FromQuery] int? categoryId,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            pageNumber = Math.Max(1, pageNumber);
            if (pageSize < 1 || pageSize > 50) pageSize = 5;

            var query = _context.SanPhams
                .Include(p => p.NhomHang)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim().ToLower();
                query = query.Where(p => p.TenSanPham.ToLower().Contains(search)
                    || p.MaVach.Contains(search));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.MaNhomHang == categoryId);
            }

            int totalItems = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Tránh tràn số Skip khi client gửi số trang quá lớn
            long offset = (long)(pageNumber - 1) * pageSize;
            var items = offset > int.MaxValue ? new List<ProductItemDto>() : await query
                .OrderBy(p => p.MaSanPham)
                .Skip((int)offset)
                .Take(pageSize)
                .Select(p => new ProductItemDto
                {
                    ProductId = p.MaSanPham,
                    Barcode = p.MaVach,
                    ProductName = p.TenSanPham,
                    Price = p.GiaBan,
                    StockQuantity = p.SoLuongTon,
                    CategoryId = p.MaNhomHang,
                    CategoryName = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                })
                .ToListAsync(cancellationToken);

            return Ok(new PagedResult<ProductItemDto>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            });
        }

        // ============================================================
        // 2. Lấy danh sách toàn bộ sản phẩm (Dùng DTO, AsNoTracking, Include)
        // GET /api/products
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.SanPhams
                .AsNoTracking()
                .Include(p => p.NhomHang)
                .Select(p => new ProductResponseDto
                {
                    MaSanPham = p.MaSanPham,
                    MaVach = p.MaVach,
                    TenSanPham = p.TenSanPham,
                    GiaBan = p.GiaBan,
                    SoLuongTon = p.SoLuongTon,
                    MaNhomHang = p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                })
                .ToListAsync();

            return Ok(products);
        }

        // ============================================================
        // 3. Lấy thông tin một sản phẩm theo mã (Dùng DTO, AsNoTracking, Include)
        // GET /api/products/{id}
        // ============================================================
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.SanPhams
                .AsNoTracking()
                .Include(p => p.NhomHang)
                .Where(p => p.MaSanPham == id)
                .Select(p => new ProductResponseDto
                {
                    MaSanPham = p.MaSanPham,
                    MaVach = p.MaVach,
                    TenSanPham = p.TenSanPham,
                    GiaBan = p.GiaBan,
                    SoLuongTon = p.SoLuongTon,
                    MaNhomHang = p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            return Ok(product);
        }

        // ============================================================
        // 4. Tra cứu nhanh sản phẩm bằng mã vạch tại quầy POS
        // GET /api/products/barcode/{barcode}
        // ============================================================
        [HttpGet("barcode/{barcode}")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            barcode = (barcode ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new { message = "Mã vạch không được để trống." });
            }

            var product = await _context.SanPhams
                .AsNoTracking()
                .Include(p => p.NhomHang)
                .Where(p => p.MaVach == barcode)
                .Select(p => new ProductResponseDto
                {
                    MaSanPham = p.MaSanPham,
                    MaVach = p.MaVach,
                    TenSanPham = p.TenSanPham,
                    GiaBan = p.GiaBan,
                    SoLuongTon = p.SoLuongTon,
                    MaNhomHang = p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm có mã vạch này." });
            }

            return Ok(product);
        }

        // ============================================================
        // 5. Tìm kiếm sản phẩm theo tên hoặc mã vạch bằng LINQ Contains
        // GET /api/products/search?keyword=...
        // ============================================================
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa." });
            }

            var products = await _context.SanPhams
                .AsNoTracking()
                .Include(p => p.NhomHang)
                .Where(p => p.TenSanPham.Contains(keyword) || p.MaVach.Contains(keyword))
                .Select(p => new ProductResponseDto
                {
                    MaSanPham = p.MaSanPham,
                    MaVach = p.MaVach,
                    TenSanPham = p.TenSanPham,
                    GiaBan = p.GiaBan,
                    SoLuongTon = p.SoLuongTon,
                    MaNhomHang = p.MaNhomHang,
                    TenNhomHang = p.NhomHang != null ? p.NhomHang.TenNhomHang : string.Empty
                })
                .ToListAsync();

            return Ok(products);
        }

        // ============================================================
        // 6. Thêm sản phẩm mới (Nhận DTO, Trả mã 201 CreatedAtAction)
        // POST /api/products
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.GiaBan <= 0)
            {
                return BadRequest(new { message = "Giá bán phải lớn hơn 0." });
            }

            if (dto.SoLuongTon < 0)
            {
                return BadRequest(new { message = "Số lượng tồn kho không được âm." });
            }

            var category = await _context.NhomHangs.FindAsync(dto.MaNhomHang);
            if (category == null)
            {
                return BadRequest(new { message = "Nhóm hàng không tồn tại." });
            }

            bool barcodeExists = await _context.SanPhams
                .AnyAsync(p => p.MaVach == dto.MaVach.Trim());

            if (barcodeExists)
            {
                return Conflict(new { message = "Mã vạch đã tồn tại." });
            }

            var product = new SanPham
            {
                MaVach = dto.MaVach.Trim(),
                TenSanPham = dto.TenSanPham.Trim(),
                GiaBan = dto.GiaBan,
                SoLuongTon = dto.SoLuongTon,
                MaNhomHang = dto.MaNhomHang
            };

            _context.SanPhams.Add(product);
            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                MaSanPham = product.MaSanPham,
                MaVach = product.MaVach,
                TenSanPham = product.TenSanPham,
                GiaBan = product.GiaBan,
                SoLuongTon = product.SoLuongTon,
                MaNhomHang = product.MaNhomHang,
                TenNhomHang = category.TenNhomHang
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.MaSanPham },
                responseDto);
        }

        // ============================================================
        // 7. Cập nhật thông tin sản phẩm (Nhận DTO)
        // PUT /api/products/{id}
        // ============================================================
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _context.SanPhams.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            if (dto.GiaBan <= 0)
            {
                return BadRequest(new { message = "Giá bán phải lớn hơn 0." });
            }

            if (dto.SoLuongTon < 0)
            {
                return BadRequest(new { message = "Số lượng tồn kho không được âm." });
            }

            var category = await _context.NhomHangs.FindAsync(dto.MaNhomHang);
            if (category == null)
            {
                return BadRequest(new { message = "Nhóm hàng không tồn tại." });
            }

            bool barcodeExists = await _context.SanPhams
                .AnyAsync(p => p.MaVach == dto.MaVach.Trim() && p.MaSanPham != id);

            if (barcodeExists)
            {
                return Conflict(new { message = "Mã vạch đã tồn tại." });
            }

            existing.MaVach = dto.MaVach.Trim();
            existing.TenSanPham = dto.TenSanPham.Trim();
            existing.GiaBan = dto.GiaBan;
            existing.SoLuongTon = dto.SoLuongTon;
            existing.MaNhomHang = dto.MaNhomHang;

            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                MaSanPham = existing.MaSanPham,
                MaVach = existing.MaVach,
                TenSanPham = existing.TenSanPham,
                GiaBan = existing.GiaBan,
                SoLuongTon = existing.SoLuongTon,
                MaNhomHang = existing.MaNhomHang,
                TenNhomHang = category.TenNhomHang
            };

            return Ok(responseDto);
        }

        // ============================================================
        // 8. Xóa sản phẩm
        // DELETE /api/products/{id}
        // ============================================================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Warehouse")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.SanPhams.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            bool usedInOrders = await _context.ChiTietDonHangs
                .AnyAsync(d => d.MaSanPham == id);

            if (usedInOrders)
            {
                return Conflict(new { message = "Sản phẩm đã có trong đơn hàng, không thể xóa." });
            }

            _context.SanPhams.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa sản phẩm thành công."
            });
        }
    }
}
