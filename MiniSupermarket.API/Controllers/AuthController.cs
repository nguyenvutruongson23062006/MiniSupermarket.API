using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniSupermarket.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // DbContext dùng để truy vấn bảng người dùng
        private readonly SupermarketDbContext _context;

        // Cấu hình ứng dụng dùng để lấy JWT Secret
        private readonly IConfiguration _configuration;

        // Khởi tạo Controller
        public AuthController(
            SupermarketDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ============================================================
        // ĐĂNG NHẬP
        // POST /api/auth/login
        // ============================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng nhập tên đăng nhập và mật khẩu!"
                });
            }

            // Tìm người dùng trong SQL Server
            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(nd =>
                    nd.TenDangNhap == request.Username &&
                    nd.MatKhau == request.Password &&
                    nd.DangHoatDong == true);

            // Không tìm thấy tài khoản
            if (nguoiDung == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Sai tài khoản hoặc mật khẩu!"
                });
            }

            // Tạo JWT Token
            var token = GenerateJwtToken(
                nguoiDung.TenDangNhap,
                nguoiDung.VaiTro);

            // Trả kết quả đăng nhập
            return Ok(new
            {
                success = true,
                token = token,
                role = nguoiDung.VaiTro,
                username = nguoiDung.TenDangNhap,
                fullName = nguoiDung.HoTen
            });
        }

        // ============================================================
        // TẠO JWT TOKEN
        // ============================================================
        private string GenerateJwtToken(
            string username,
            string role)
        {
            var tokenHandler =
                new JwtSecurityTokenHandler();

            // Lấy khóa bí mật từ appsettings.json
            var key = Encoding.ASCII.GetBytes(
                _configuration["JwtSettings:Secret"] ??
                "SupermarketSecretKeyDoAnMonHoc2026SecureString!!");

            // Cấu hình thông tin Token
            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new[]
                        {
                            new Claim(
                                ClaimTypes.Name,
                                username),

                            new Claim(
                                ClaimTypes.Role,
                                role)
                        }),

                    // Token có thời hạn 2 tiếng
                    Expires = DateTime.UtcNow.AddHours(2),

                    // Khóa ký JWT
                    SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                };

            // Tạo Token
            var token =
                tokenHandler.CreateToken(tokenDescriptor);

            // Chuyển Token thành chuỗi
            return tokenHandler.WriteToken(token);
        }
    }

    // ================================================================
    // DTO DÙNG CHO CHỨC NĂNG ĐĂNG NHẬP
    // ================================================================
    public class LoginRequestDto
    {
        // Tên đăng nhập
        public string Username { get; set; } =
            string.Empty;

        // Mật khẩu
        public string Password { get; set; } =
            string.Empty;
    }
}