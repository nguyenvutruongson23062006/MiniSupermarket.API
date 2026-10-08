using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng khách hàng
    [Table("Customers")]
    public class Customer
    {
        // Khóa chính của khách hàng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        // Họ và tên khách hàng
        [Required(ErrorMessage = "Họ tên khách hàng không được để trống")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Số điện thoại khách hàng
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        // Email khách hàng
        [StringLength(100)]
        public string? Email { get; set; }

        // Địa chỉ khách hàng
        [StringLength(255)]
        public string? Address { get; set; }

        // Danh sách đơn hàng của khách hàng
        public virtual ICollection<Order>? Orders { get; set; }
    }
}