using System.Data;
using Microsoft.Data.SqlClient;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Services;

// Cùng một kết nối và transaction cho toàn bộ nghiệp vụ thanh toán/hủy đơn.
public sealed class AdoCheckoutService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration
        .GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Thiếu chuỗi kết nối database.");

    private static SqlCommand Command(SqlConnection connection,
        SqlTransaction transaction, string sql, params SqlParameter[] parameters)
    {
        var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddRange(parameters);
        return command;
    }

    private static SqlParameter Number(string name, int value) =>
        new(name, SqlDbType.Int) { Value = value };

    private static SqlParameter Money(string name, decimal value) =>
        new(name, SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = value };

    public async Task<CheckoutResult> CheckoutAsync(
        CheckoutRequestDto request, string username, CancellationToken ct)
    {
        if (request.Items == null || request.Items.Count is 0 or > 200 ||
            request.Items.Any(x => x == null || x.ProductId <= 0 ||
                x.Quantity <= 0 || x.Quantity > 1000000) ||
            request.Items.GroupBy(x => x.ProductId).Any(g => g.Count() > 1))
            throw new InvalidOperationException("Giỏ hàng rỗng, trùng sản phẩm hoặc số lượng không hợp lệ.");
        if (request.CashierUsername != null &&
            !string.Equals(request.CashierUsername, username, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Thu ngân phải khớp tài khoản đang đăng nhập.");
        if (request.CustomerId <= 0)
            throw new InvalidOperationException("Mã khách hàng không hợp lệ.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            using var employeeCommand = Command(connection, transaction,
                "SELECT MaNguoiDung FROM NguoiDung WHERE TenDangNhap=@User AND DangHoatDong=1 AND VaiTro IN ('Admin','Cashier')",
                new SqlParameter("@User", SqlDbType.NVarChar, 50) { Value = username });
            var employee = await employeeCommand.ExecuteScalarAsync(ct)
                ?? throw new InvalidOperationException("Tài khoản không tồn tại hoặc đã bị khóa.");

            int? customerId = request.CustomerId;
            if (customerId.HasValue || !string.IsNullOrWhiteSpace(request.CustomerPhone))
            {
                using var customerCommand = Command(connection, transaction,
                    "SELECT MaKhachHang FROM KhachHang WITH (UPDLOCK, HOLDLOCK) WHERE " +
                    (customerId.HasValue ? "MaKhachHang=@Id" : "SoDienThoai=@Phone"),
                    customerId.HasValue ? Number("@Id", customerId.Value) :
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 15) { Value = request.CustomerPhone!.Trim() });
                customerId = Convert.ToInt32(await customerCommand.ExecuteScalarAsync(ct)
                    ?? throw new InvalidOperationException("Không tìm thấy khách hàng."));
            }

            string code = $"HD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}";
            using var orderCommand = Command(connection, transaction, """
                INSERT INTO DonHang (MaKhachHang, MaNguoiDung, NgayDatHang, TongTien,
                    TrangThai, MaHoaDon, TongSoLuong, DiemThuong)
                OUTPUT INSERTED.MaDonHang
                VALUES (@Customer, @Employee, GETDATE(), 0, 'Completed', @Code, 0, 0)
                """,
                new SqlParameter("@Customer", SqlDbType.Int) { Value = (object?)customerId ?? DBNull.Value },
                Number("@Employee", Convert.ToInt32(employee)),
                new SqlParameter("@Code", SqlDbType.NVarChar, 50) { Value = code });
            int orderId = Convert.ToInt32(await orderCommand.ExecuteScalarAsync(ct));
            decimal total = 0;
            int quantity = 0;

            // Khóa theo thứ tự mã cố định giảm nguy cơ deadlock giữa nhiều quầy.
            foreach (var item in request.Items.OrderBy(x => x.ProductId))
            {
                decimal price;
                using (var productCommand = Command(connection, transaction, """
                    SELECT TenSanPham, GiaBan, SoLuongTon FROM SanPham WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaSanPham=@Id
                    """, Number("@Id", item.ProductId)))
                await using (var reader = await productCommand.ExecuteReaderAsync(ct))
                {
                    if (!await reader.ReadAsync(ct))
                        throw new InvalidOperationException($"Không tìm thấy sản phẩm {item.ProductId}.");
                    price = reader.GetDecimal(1);
                    if (reader.GetInt32(2) < item.Quantity)
                        throw new InvalidOperationException($"Sản phẩm '{reader.GetString(0)}' không đủ tồn kho.");
                    if (price < 0 || (item.UnitPrice.HasValue && item.UnitPrice.Value != price))
                        throw new InvalidOperationException("Giá sản phẩm đã thay đổi hoặc không hợp lệ. Vui lòng tải lại giỏ hàng.");
                }

                decimal lineTotal = checked(price * item.Quantity);
                total = checked(total + lineTotal);
                quantity = checked(quantity + item.Quantity);
                using var detailCommand = Command(connection, transaction, """
                    INSERT INTO ChiTietDonHang (MaDonHang, MaSanPham, SoLuong, DonGia, ThanhTien)
                    VALUES (@Order, @Product, @Quantity, @Price, @Total)
                    """, Number("@Order", orderId), Number("@Product", item.ProductId),
                    Number("@Quantity", item.Quantity), Money("@Price", price), Money("@Total", lineTotal));
                await detailCommand.ExecuteNonQueryAsync(ct);

                using var stockCommand = Command(connection, transaction, """
                    UPDATE SanPham SET SoLuongTon=SoLuongTon-@Quantity
                    WHERE MaSanPham=@Id AND SoLuongTon>=@Quantity
                    """, Number("@Quantity", item.Quantity), Number("@Id", item.ProductId));
                if (await stockCommand.ExecuteNonQueryAsync(ct) != 1)
                    throw new InvalidOperationException("Tồn kho thay đổi, vui lòng thử lại.");
            }

            int points = customerId.HasValue ? checked((int)(total / 10000m)) : 0;
            using var finishCommand = Command(connection, transaction, """
                UPDATE DonHang SET TongTien=@Total, TongSoLuong=@Quantity, DiemThuong=@Points
                WHERE MaDonHang=@Id
                """, Money("@Total", total), Number("@Quantity", quantity),
                Number("@Points", points), Number("@Id", orderId));
            await finishCommand.ExecuteNonQueryAsync(ct);
            if (customerId.HasValue)
            {
                using var pointsCommand = Command(connection, transaction,
                    "UPDATE KhachHang SET DiemTichLuy=DiemTichLuy+@Points WHERE MaKhachHang=@Id",
                    Number("@Points", points), Number("@Id", customerId.Value));
                if (await pointsCommand.ExecuteNonQueryAsync(ct) != 1)
                    throw new InvalidOperationException("Không cập nhật được điểm khách hàng.");
            }
            transaction.Commit();
            return new CheckoutResult(orderId, code, quantity, total, points);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task CancelAsync(int orderId, string username, bool isAdmin, CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            using var activeCommand = Command(connection, transaction,
                "SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap=@User AND DangHoatDong=1 AND VaiTro IN ('Admin','Cashier')",
                new SqlParameter("@User", SqlDbType.NVarChar, 50) { Value = username });
            if (Convert.ToInt32(await activeCommand.ExecuteScalarAsync(ct)) != 1)
                throw new InvalidOperationException("Tài khoản đã bị khóa hoặc không còn quyền bán hàng.");

            int? customer;
            int points;
            using (var command = Command(connection, transaction, """
                SELECT d.MaKhachHang, d.DiemThuong, u.TenDangNhap
                FROM DonHang d WITH (UPDLOCK, HOLDLOCK)
                JOIN NguoiDung u ON u.MaNguoiDung=d.MaNguoiDung
                WHERE d.MaDonHang=@Id
                """, Number("@Id", orderId)))
            await using (var reader = await command.ExecuteReaderAsync(ct))
            {
                if (!await reader.ReadAsync(ct))
                    throw new KeyNotFoundException("Không tìm thấy hóa đơn để hủy.");
                if (!isAdmin && !string.Equals(reader.GetString(2), username, StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("Thu ngân chỉ được hủy hóa đơn do mình lập.");
                customer = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                points = reader.GetInt32(1);
            }

            if (customer.HasValue)
            {
                using var customerCommand = Command(connection, transaction, """
                    UPDATE KhachHang SET DiemTichLuy=CASE WHEN DiemTichLuy>=@Points
                        THEN DiemTichLuy-@Points ELSE 0 END WHERE MaKhachHang=@Id
                    """, Number("@Points", points), Number("@Id", customer.Value));
                if (await customerCommand.ExecuteNonQueryAsync(ct) != 1)
                    throw new InvalidOperationException("Không tìm thấy khách hàng của hóa đơn.");
            }

            var items = new List<(int Id, int Quantity)>();
            using (var details = Command(connection, transaction, """
                SELECT MaSanPham, SUM(SoLuong) FROM ChiTietDonHang
                WHERE MaDonHang=@Id GROUP BY MaSanPham ORDER BY MaSanPham
                """, Number("@Id", orderId)))
            await using (var reader = await details.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                    items.Add((reader.GetInt32(0), reader.GetInt32(1)));
            }
            foreach (var item in items)
            {
                using var restore = Command(connection, transaction,
                    "UPDATE SanPham SET SoLuongTon=SoLuongTon+@Quantity WHERE MaSanPham=@Id",
                    Number("@Quantity", item.Quantity), Number("@Id", item.Id));
                if (await restore.ExecuteNonQueryAsync(ct) != 1)
                    throw new InvalidOperationException("Không tìm thấy sản phẩm cần hoàn kho.");
            }
            using var delete = Command(connection, transaction, """
                DELETE FROM ChiTietDonHang WHERE MaDonHang=@Id;
                DELETE FROM DonHang WHERE MaDonHang=@Id;
                """, Number("@Id", orderId));
            await delete.ExecuteNonQueryAsync(ct);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
