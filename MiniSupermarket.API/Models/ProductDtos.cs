using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Models;

// DTO trả về thông tin sản phẩm
public class ProductResponseDto
{
    public int MaSanPham { get; set; }
    public string MaVach { get; set; } = string.Empty;
    public string TenSanPham { get; set; } = string.Empty;
    public decimal GiaBan { get; set; }
    public int SoLuongTon { get; set; }
    public int MaNhomHang { get; set; }
    public string TenNhomHang { get; set; } = string.Empty;
}

// DTO tiếp nhận dữ liệu thêm mới sản phẩm
public class CreateProductDto
{
    [Required(ErrorMessage = "Mã vạch sản phẩm không được để trống")]
    [StringLength(50)]
    public string MaVach { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(150)]
    public string TenSanPham { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá bán không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0!")]
    public decimal GiaBan { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm!")]
    public int SoLuongTon { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã nhóm hàng không hợp lệ!")]
    public int MaNhomHang { get; set; }
}

// DTO tiếp nhận dữ liệu cập nhật sản phẩm
public class UpdateProductDto
{
    [Required(ErrorMessage = "Mã vạch sản phẩm không được để trống")]
    [StringLength(50)]
    public string MaVach { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(150)]
    public string TenSanPham { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá bán không được để trống")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0!")]
    public decimal GiaBan { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm!")]
    public int SoLuongTon { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã nhóm hàng không hợp lệ!")]
    public int MaNhomHang { get; set; }
}
