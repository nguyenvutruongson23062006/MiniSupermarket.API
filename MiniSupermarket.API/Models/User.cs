using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng người dùng
    [Table("NguoiDung")]
    public class NguoiDung
    {
        // Mã người dùng - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNguoiDung { get; set; }

        // Tên đăng nhập
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        // Mật khẩu
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        // Họ tên người dùng
        [StringLength(100)]
        public string? HoTen { get; set; }

        // Vai trò: Admin hoặc Cashier
        [Required]
        [StringLength(50)]
        public string VaiTro { get; set; } = "Cashier";

        // Trạng thái hoạt động của tài khoản
        public bool DangHoatDong { get; set; } = true;

        // Danh sách đơn hàng do người dùng lập
        public virtual ICollection<DonHang>? DonHangs { get; set; }
    }
}