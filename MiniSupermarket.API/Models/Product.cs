using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng sản phẩm
    [Table("SanPham")]
    public class SanPham
    {
        // Mã sản phẩm - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaSanPham { get; set; }

        // Mã vạch sản phẩm
        [Required(ErrorMessage = "Mã vạch sản phẩm không được để trống")]
        [StringLength(50)]
        public string MaVach { get; set; } = string.Empty;

        // Tên sản phẩm
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string TenSanPham { get; set; } = string.Empty;

        // Giá bán
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaBan { get; set; }

        // Số lượng tồn kho
        public int SoLuongTon { get; set; }

        // Mã nhóm hàng - Khóa ngoại
        public int MaNhomHang { get; set; }

        // Nhóm hàng của sản phẩm
        [ForeignKey("MaNhomHang")]
        public virtual NhomHang? NhomHang { get; set; }
    }
}