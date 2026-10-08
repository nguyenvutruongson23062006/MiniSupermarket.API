using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng nhóm hàng
    [Table("NhomHang")]
    public class NhomHang
    {
        // Mã nhóm hàng - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNhomHang { get; set; }

        // Tên nhóm hàng
        [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên nhóm hàng không vượt quá 100 ký tự")]
        public string TenNhomHang { get; set; } = string.Empty;

        // Mô tả nhóm hàng
        [StringLength(255)]
        public string? MoTa { get; set; }

        // Quan hệ 1 - N:
        // Một nhóm hàng có thể có nhiều sản phẩm
        [JsonIgnore]
        public virtual ICollection<SanPham>? SanPhams { get; set; }
    }
}