using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng sản phẩm (Entity Product)
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

        // Giá bán (Ràng buộc: giá bán > 0)
        [Required(ErrorMessage = "Giá bán không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaBan { get; set; }

        // Số lượng tồn kho
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm!")]
        public int SoLuongTon { get; set; }

        // Mã nhóm hàng - Khóa ngoại
        public int MaNhomHang { get; set; }

        // Nhóm hàng của sản phẩm
        [ForeignKey("MaNhomHang")]
        public virtual NhomHang? NhomHang { get; set; }
    }
}
