using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng chi tiết đơn hàng
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        // Mã chi tiết đơn hàng - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaChiTietDonHang { get; set; }

        // Mã đơn hàng - Khóa ngoại
        public int MaDonHang { get; set; }

        // Đơn hàng chứa sản phẩm này
        [ForeignKey("MaDonHang")]
        public virtual DonHang? DonHang { get; set; }

        // Mã sản phẩm - Khóa ngoại
        public int MaSanPham { get; set; }

        // Sản phẩm được mua
        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; }

        // Số lượng sản phẩm
        public int SoLuong { get; set; }

        // Đơn giá tại thời điểm mua
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        // Thành tiền
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien { get; set; }
    }
}