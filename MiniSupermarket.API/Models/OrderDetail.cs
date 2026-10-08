using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng chi tiết đơn hàng
    [Table("OrderDetails")]
    public class OrderDetail
    {
        // Khóa chính của chi tiết đơn hàng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        // Mã đơn hàng
        public int OrderId { get; set; }

        // Đơn hàng chứa sản phẩm này
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        // Mã sản phẩm
        public int ProductId { get; set; }

        // Sản phẩm được mua
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        // Số lượng sản phẩm
        public int Quantity { get; set; }

        // Đơn giá tại thời điểm mua
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Thành tiền
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}