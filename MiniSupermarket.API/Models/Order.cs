using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng đơn hàng
    [Table("Orders")]
    public class Order
    {
        // Khóa chính của đơn hàng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // Mã khách hàng
        public int CustomerId { get; set; }

        // Khách hàng thực hiện đơn hàng
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        // Mã người dùng lập đơn
        public int UserId { get; set; }

        // Người dùng lập đơn hàng
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        // Ngày tạo đơn hàng
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Tổng tiền đơn hàng
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Trạng thái đơn hàng
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Danh sách sản phẩm trong đơn hàng
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}