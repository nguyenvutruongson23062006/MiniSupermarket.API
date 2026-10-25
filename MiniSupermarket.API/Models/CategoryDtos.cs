using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Models;

// DTO trả thông tin nhóm hàng
public class CategoryDto
{
    public int MaNhomHang { get; set; }
    public string TenNhomHang { get; set; } = string.Empty;
    public string? MoTa { get; set; }
}

// DTO tiếp nhận dữ liệu tạo mới nhóm hàng
public class CreateCategoryDto
{
    [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
    [StringLength(100, ErrorMessage = "Tên nhóm hàng không vượt quá 100 ký tự")]
    public string TenNhomHang { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }
}

// DTO tiếp nhận dữ liệu cập nhật nhóm hàng
public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
    [StringLength(100, ErrorMessage = "Tên nhóm hàng không vượt quá 100 ký tự")]
    public string TenNhomHang { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }
}
