using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Models;

public class CartItemCheckoutDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, 1000000)]
    public int Quantity { get; set; }

    // Giá client chỉ dùng kiểm tra thay đổi giá; giá tính tiền luôn lấy từ SQL.
    [Range(typeof(decimal), "0", "9999999999999999")]
    public decimal? UnitPrice { get; set; }
}

public class CheckoutRequestDto
{
    public string? CashierUsername { get; set; }
    [Range(1, int.MaxValue)]
    public int? CustomerId { get; set; }
    public string? CustomerPhone { get; set; }

    [Required, MinLength(1), MaxLength(200)]
    public List<CartItemCheckoutDto> Items { get; set; } = new();
}

public record CheckoutResult(
    int OrderId, string OrderCode, int TotalItems,
    decimal FinalAmount, int RewardPointsAdded);
