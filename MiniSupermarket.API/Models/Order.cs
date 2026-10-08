using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model đại diện cho bảng đơn hàng
    [Table("DonHang")]
    public class DonHang
    {
        // Mã đơn hàng - Khóa chính
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDonHang { get; set; }

        // Mã khách hàng - Khóa ngoại
        public int MaKhachHang { get; set; }

        // Khách hàng thực hiện đơn hàng
        [ForeignKey("MaKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }

        // Mã người dùng lập đơn - Khóa ngoại
        public int MaNguoiDung { get; set; }

        // Người dùng lập đơn hàng
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung? NguoiDung { get; set; }

        // Ngày tạo đơn hàng
        public DateTime NgayDatHang { get; set; } = DateTime.Now;

        // Tổng tiền đơn hàng
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        // Trạng thái đơn hàng
        [StringLength(50)]
        public string TrangThai { get; set; } = "Pending";

        // Danh sách chi tiết đơn hàng
        public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
    }
}