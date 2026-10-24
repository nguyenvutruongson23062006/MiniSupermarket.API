using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSupermarket.API.Models;
using MiniSupermarket.API.Services;

namespace MiniSupermarket.API.Controllers;

[ApiController]
[Route("api/checkout")]
[Authorize(Roles = "Admin,Cashier")]
public class CheckoutController(AdoCheckoutService checkout, ILogger<CheckoutController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CheckoutRequestDto request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(User.Identity?.Name)) return Unauthorized();
        try
        {
            var result = await checkout.CheckoutAsync(request, User.Identity.Name, ct);
            return Ok(new { success = true, message = "Thanh toán thành công!",
                result.OrderId, result.OrderCode, result.TotalItems,
                result.FinalAmount, result.RewardPointsAdded });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false,
                message = "Giao dịch thanh toán bị hủy và đã được Rollback an toàn!",
                errorDetail = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Thanh toán ADO.NET thất bại");
            return StatusCode(500, new { success = false, message = "Không thể thanh toán. Vui lòng kiểm tra kết nối và thử lại." });
        }
    }

    [HttpDelete("cancel/{orderId:int:min(1)}")]
    public async Task<IActionResult> Cancel(int orderId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(User.Identity?.Name)) return Unauthorized();
        try
        {
            await checkout.CancelAsync(orderId, User.Identity.Name, User.IsInRole("Admin"), ct);
            return Ok(new { success = true, message = "Đã hủy hóa đơn, hoàn kho và trừ lại điểm thưởng." });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Hủy hóa đơn {OrderId} thất bại", orderId);
            return StatusCode(500, new { message = "Không thể hủy đơn. Vui lòng thử lại." });
        }
    }
}
