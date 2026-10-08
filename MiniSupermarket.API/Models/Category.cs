using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Categories")]
    public class Category
    {
        // Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        // Tên nhóm hàng
        [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên nhóm hàng không vượt quá 100 ký tự")]
        public string CategoryName { get; set; } = string.Empty;

        // Mô tả nhóm hàng
        [StringLength(255)]
        public string? Description { get; set; }

        // Quan hệ 1 - N:
        // Một nhóm hàng có thể có nhiều sản phẩm
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}