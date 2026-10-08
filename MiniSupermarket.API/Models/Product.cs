using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    [Table("Products")]
    public class Product
    {
        // Mã sản phẩm - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        // Mã vạch sản phẩm
        [Required(ErrorMessage = "Mã vạch sản phẩm không được trống")]
        [StringLength(50)]
        public string Barcode { get; set; } = string.Empty;

        // Tên sản phẩm
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        // Giá bán
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Số lượng tồn kho
        public int StockQuantity { get; set; }

        // Khóa ngoại liên kết tới bảng Categories
        public int CategoryId { get; set; }

        // Quan hệ với Category
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}