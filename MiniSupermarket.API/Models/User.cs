using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng tài khoản người dùng
    [Table("Users")]
    public class User
    {
        // Khóa chính của người dùng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // Tên đăng nhập
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        // Mật khẩu
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        // Họ tên người dùng
        [StringLength(100)]
        public string? FullName { get; set; }

        // Vai trò: Admin hoặc Cashier
        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "Cashier";

        // Trạng thái hoạt động của tài khoản
        public bool IsActive { get; set; } = true;

        // Danh sách đơn hàng do người dùng lập
        public virtual ICollection<Order>? Orders { get; set; }
    }
}