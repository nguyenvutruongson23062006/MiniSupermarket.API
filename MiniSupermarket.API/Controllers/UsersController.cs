using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private static readonly string[] AllowedRoles =
        [
            "Admin",
            "Cashier",
            "Warehouse"
        ];

        private readonly SupermarketDbContext _context;

        public UsersController(SupermarketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.NguoiDungs
                .AsNoTracking()
                .OrderBy(x => x.MaNguoiDung)
                .Select(x => new
                {
                    x.MaNguoiDung,
                    x.TenDangNhap,
                    x.HoTen,
                    x.Email,
                    x.VaiTro,
                    x.DangHoatDong
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            string username = request.TenDangNhap.Trim();
            string role = NormalizeRole(request.VaiTro);

            if (string.IsNullOrEmpty(role))
                return BadRequest("Vai trò không hợp lệ.");

            bool usernameExists = await _context.NguoiDungs
                .AnyAsync(x => x.TenDangNhap == username);

            if (usernameExists)
                return Conflict("Tên đăng nhập đã tồn tại.");

            var user = new NguoiDung
            {
                TenDangNhap = username,
                MatKhau = request.MatKhau,
                HoTen = request.HoTen.Trim(),
                Email = string.IsNullOrWhiteSpace(request.Email)
                    ? null
                    : request.Email.Trim(),
                VaiTro = role,
                DangHoatDong = true
            };

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    user.MaNguoiDung,
                    user.TenDangNhap,
                    user.HoTen,
                    user.Email,
                    user.VaiTro,
                    user.DangHoatDong
                });
        }

        [HttpPut("{id:int}/reset-password")]
        public async Task<IActionResult> ResetPassword(
            int id,
            [FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _context.NguoiDungs.FindAsync(id);

            if (user == null)
                return NotFound("Không tìm thấy tài khoản.");

            user.MatKhau = request.MatKhau;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đặt lại mật khẩu thành công."
            });
        }

        [HttpPut("{id:int}/toggle-lock")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var user = await _context.NguoiDungs.FindAsync(id);

            if (user == null)
                return NotFound("Không tìm thấy tài khoản.");

            string? currentUsername =
                User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.Equals(
                user.TenDangNhap,
                currentUsername,
                StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "Không thể khóa chính tài khoản đang đăng nhập.");
            }

            user.DangHoatDong = !user.DangHoatDong;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.MaNguoiDung,
                user.TenDangNhap,
                user.DangHoatDong,
                message = user.DangHoatDong
                    ? "Đã mở khóa tài khoản."
                    : "Đã khóa tài khoản."
            });
        }

        private static string NormalizeRole(string role)
        {
            return AllowedRoles.FirstOrDefault(x =>
                string.Equals(
                    x,
                    role?.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                ?? string.Empty;
        }
    }

    public class CreateUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public string VaiTro { get; set; } = "Cashier";
    }

    public class ResetPasswordRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;
    }
}
