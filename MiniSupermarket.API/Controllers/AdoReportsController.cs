using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin")]
public class AdoReportsController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("revenue-by-cashier")]
    public async Task<IActionResult> ByCashier(CancellationToken ct)
    {
        var rows = await ReadProcedureAsync("dbo.sp_GetRevenueByCashier", [], ct);
        var data = rows.Select(row => new CashierRevenueDto(
            Convert.ToString(row["CashierUsername"]) ?? string.Empty,
            Convert.ToInt32(row["TotalOrders"]), Convert.ToDecimal(row["TotalRevenue"] ?? 0),
            Convert.ToInt32(row["TotalPointsAwarded"] ?? 0))).ToList();
        return Ok(new { success = true, generatedAt = DateTime.Now,
            totalCashiers = data.Count,
            grandTotalRevenue = data.Sum(x => x.TotalRevenue), data });
    }

    [HttpGet("revenue-by-date-range")]
    public async Task<IActionResult> ByDateRange(DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        if (fromDate == default || toDate == default || fromDate.Date > toDate.Date || toDate.Date == DateTime.MaxValue.Date)
            return BadRequest(new { message = "Nhập khoảng ngày hợp lệ, ngày bắt đầu không lớn hơn ngày kết thúc." });
        return Ok(await ReadProcedureAsync("dbo.sp_GetRevenueByDateRange", [
            new SqlParameter("@FromDate", SqlDbType.DateTime2) { Value = fromDate.Date },
            new SqlParameter("@ToDate", SqlDbType.DateTime2) { Value = toDate.Date }], ct));
    }

    [HttpGet("top-selling-products")]
    public async Task<IActionResult> TopProducts(CancellationToken ct) =>
        Ok(await ReadProcedureAsync("dbo.sp_GetTop5SellingProducts", [], ct));

    private async Task<List<Dictionary<string, object?>>> ReadProcedureAsync(
        string procedure, SqlParameter[] parameters, CancellationToken ct)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        using var command = new SqlCommand(procedure, connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddRange(parameters);
        await connection.OpenAsync(ct);
        await using var reader = await command.ExecuteReaderAsync(ct);
        var rows = new List<Dictionary<string, object?>>();
        while (await reader.ReadAsync(ct))
        {
            var row = new Dictionary<string, object?>();
            for (int i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
            rows.Add(row);
        }
        return rows;
    }
}
