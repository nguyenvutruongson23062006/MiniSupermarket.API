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

    // Connected: Đọc trực tiếp bằng SqlDataReader và SqlCommand, dùng Parameters.AddWithValue chống SQL Injection
    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> Barcode(string barcode, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(barcode) || barcode.Length > 50)
            return BadRequest(new { message = "Mã vạch phải có từ 1 đến 50 ký tự." });

        await using var connection = Connection();
        using var command = new SqlCommand(
            $"SELECT {Columns} FROM SanPham WHERE MaVach = @Barcode", connection);

        // Chống tấn công SQL Injection bằng Parameters.AddWithValue
        command.Parameters.AddWithValue("@Barcode", barcode.Trim());

        await connection.OpenAsync(ct);

        // Đọc dữ liệu bằng SqlDataReader bọc trong using an toàn
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            return NotFound(new { message = "Không tìm thấy mã vạch." });
        }

        var product = new ProductLookupDto(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetDecimal(3),
            reader.GetInt32(4));

        return Ok(product);
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
    public async Task<IActionResult> DisconnectedCache()
    {
        using var table = await Task.Run(() => LoadTable($"SELECT {Columns} FROM SanPham ORDER BY MaSanPham"));
        var data = table.Rows.Cast<DataRow>().Select(row => new ProductLookupDto(
            (int)row[0], (string)row[1], (string)row[2], (decimal)row[3], (int)row[4])).ToList();
        return Ok(new { totalLoaded = data.Count, cachedAt = DateTime.Now, data });
    }

    [HttpGet("count-by-category")]
    [Authorize(Roles = "Admin,Warehouse")]
    public async Task<IActionResult> CountByCategory()
    {
        using var table = await Task.Run(() => LoadTable("SELECT MaNhomHang, COUNT(*) AS Total FROM SanPham GROUP BY MaNhomHang ORDER BY MaNhomHang"));
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
