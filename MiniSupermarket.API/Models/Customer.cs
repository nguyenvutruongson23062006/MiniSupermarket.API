using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng khách hàng
    [Table("KhachHang")]
    public class KhachHang
    {
        // Mã khách hàng - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaKhachHang { get; set; }

        // Họ và tên khách hàng
        [Required(ErrorMessage = "Họ tên khách hàng không được để trống")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        // Số điện thoại khách hàng
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        // Email khách hàng
        [StringLength(100)]
        public string? Email { get; set; }

        // Địa chỉ chi tiết của khách hàng
        // Bao gồm số nhà, đường, phường/xã,
        // quận/huyện, thành phố/tỉnh
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(500)]
        public string DiaChi { get; set; } = string.Empty;

        // Danh sách đơn hàng của khách hàng
        public virtual ICollection<DonHang>? DonHangs { get; set; }
    }
}