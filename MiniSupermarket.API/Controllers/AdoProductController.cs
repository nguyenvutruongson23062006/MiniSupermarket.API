using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MiniSupermarket.API.Controllers;

[ApiController]
[Route("api/ado-products")]
[Authorize(Roles = "Admin,Cashier,Warehouse")]
public class AdoProductController(IConfiguration configuration) : ControllerBase
{
    private const string Columns = "MaSanPham AS ProductId, MaVach AS Barcode, TenSanPham AS ProductName, GiaBan AS Price, SoLuongTon AS StockQuantity";
    private SqlConnection Connection() => new(configuration.GetConnectionString("DefaultConnection"));

    // Connected: đọc từng dòng khi kết nối còn mở.
    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> Barcode(string barcode, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(barcode) || barcode.Length > 50)
            return BadRequest(new { message = "Mã vạch phải có từ 1 đến 50 ký tự." });
        var rows = await ReadAsync($"SELECT {Columns} FROM SanPham WHERE MaVach=@Barcode",
            new SqlParameter("@Barcode", SqlDbType.NVarChar, 50) { Value = barcode.Trim() }, ct);
        return rows.Count == 0
            ? NotFound(new { message = "Không tìm thấy mã vạch." })
            : Ok(rows[0]);
    }

    [HttpGet("filter-stock")]
    [Authorize(Roles = "Admin,Warehouse")]
    public async Task<IActionResult> FilterStock([Range(0, int.MaxValue)] int minStock = 50,
        CancellationToken ct = default) => Ok(await ReadAsync(
            $"SELECT {Columns} FROM SanPham WHERE SoLuongTon<=@MinStock ORDER BY SoLuongTon, MaSanPham",
            new SqlParameter("@MinStock", SqlDbType.Int) { Value = minStock }, ct));

    private async Task<List<ProductLookupDto>> ReadAsync(string sql, SqlParameter parameter, CancellationToken ct)
    {
        await using var connection = Connection();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(parameter);
        await connection.OpenAsync(ct);
        await using var reader = await command.ExecuteReaderAsync(ct);
        var items = new List<ProductLookupDto>();
        while (await reader.ReadAsync(ct))
            items.Add(new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                reader.GetDecimal(3), reader.GetInt32(4)));
        return items;
    }

    // Disconnected: Fill mở/đóng kết nối; DataTable còn dùng được sau Dispose.
    [HttpGet("disconnected-cache")]
    public IActionResult DisconnectedCache()
    {
        using var table = LoadTable($"SELECT {Columns} FROM SanPham ORDER BY MaSanPham");
        var data = table.Rows.Cast<DataRow>().Select(row => new ProductLookupDto(
            (int)row[0], (string)row[1], (string)row[2], (decimal)row[3], (int)row[4])).ToList();
        return Ok(new { totalLoaded = data.Count, cachedAt = DateTime.Now, data });
    }

    [HttpGet("count-by-category")]
    [Authorize(Roles = "Admin,Warehouse")]
    public IActionResult CountByCategory()
    {
        using var table = LoadTable("SELECT MaNhomHang, COUNT(*) AS Total FROM SanPham GROUP BY MaNhomHang ORDER BY MaNhomHang");
        return Ok(table.Rows.Cast<DataRow>().Select(row => new {
            categoryId = (int)row[0], total = (int)row[1] }).ToList());
    }

    private DataTable LoadTable(string sql)
    {
        var table = new DataTable();
        using var connection = Connection();
        using var command = new SqlCommand(sql, connection);
        using var adapter = new SqlDataAdapter(command);
        adapter.Fill(table);
        return table;
    }
}

public record ProductLookupDto(int ProductId, string Barcode, string ProductName,
    decimal Price, int StockQuantity);
