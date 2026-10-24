using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ThucHanhTransaction_Tiet4;

internal static class Program
{
    private static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            DemonstrateRollback();
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("-> Nhấn phím bất kỳ để kết thúc Tiết 4...");
                Console.ReadKey(true);
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[LỖI]: {ex.Message}");
            return 1;
        }
    }

    private static string GetConnectionString()
    {
        string? configured = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(configured)) return configured;
        using var settings = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.json")));
        return settings.RootElement.GetProperty("ConnectionStrings")
            .GetProperty("DefaultConnection").GetString()
            ?? throw new InvalidOperationException("Thiếu chuỗi kết nối SQL Server.");
    }

    private static void DemonstrateRollback()
    {
        // ACID trong nghiệp vụ bán hàng:
        // Atomicity: các bước cùng thành công hoặc cùng được rollback.
        // Consistency: tồn kho không âm và dữ liệu giữ đúng các ràng buộc.
        // Isolation: UPDLOCK/HOLDLOCK giữ dòng tồn kho trong suốt giao dịch.
        // Durability: chỉ Commit mới xác nhận lưu các thay đổi thành công.
        Console.WriteLine("=== THỬ NGHIỆM ĐIỀU KHIỂN SQLTRANSACTION ===");
        using var connection = new SqlConnection(GetConnectionString());
        connection.Open();
        using var transaction = connection.BeginTransaction();
        int before = 0;
        bool simulatedShortage = false;
        try
        {
            Console.WriteLine("-> Bắt đầu phiên giao dịch...");
            using (var read = new SqlCommand(
                "SELECT SoLuongTon FROM dbo.SanPham WITH (UPDLOCK, HOLDLOCK) WHERE MaSanPham=@Id",
                connection, transaction))
            {
                read.Parameters.Add("@Id", SqlDbType.Int).Value = 1;
                object? value = read.ExecuteScalar();
                if (value is null or DBNull)
                    throw new InvalidOperationException("Không có sản phẩm ID = 1 để thực hành.");
                before = Convert.ToInt32(value);
            }
            Console.WriteLine($"-> Tồn kho sản phẩm 1 trước giao dịch: {before}.");

            // Mọi command đều dùng cùng connection và transaction.
            using (var update = new SqlCommand("""
                UPDATE dbo.SanPham SET SoLuongTon=SoLuongTon-@Qty
                WHERE MaSanPham=@Id AND SoLuongTon>=@Qty
                """, connection, transaction))
            {
                update.Parameters.Add("@Qty", SqlDbType.Int).Value = 5;
                update.Parameters.Add("@Id", SqlDbType.Int).Value = 1;
                int affected = update.ExecuteNonQuery();
                if (affected != 1)
                    throw new InvalidOperationException("Sản phẩm 1 cần có ít nhất 5 đơn vị tồn kho để chạy bài này.");
                Console.WriteLine($"-> Bước 1: Đã trừ 5 đơn vị của sản phẩm 1. Số dòng tác động: {affected}.");
            }

            Console.WriteLine("-> Bước 2: Chuẩn bị thực thi thao tác lỗi giả lập...");
            bool isInventoryShortage = true;
            if (isInventoryShortage)
            {
                simulatedShortage = true;
                throw new InvalidOperationException("Mặt hàng thứ hai đã hết tồn kho thực tế!");
            }

            // Chỉ commit khi toàn bộ thao tác thành công. Bài mô phỏng sẽ vào catch.
            transaction.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PHÁT SINH LỖI]: {ex.Message}");
            try
            {
                transaction.Rollback();
                Console.WriteLine("-> Đã kích hoạt transaction.Rollback()!");
            }
            catch (Exception rollbackError)
            {
                throw new InvalidOperationException("Không rollback được giao dịch.", rollbackError);
            }
            if (!simulatedShortage) throw;
        }

        // Đọc lại sau rollback để chứng minh dữ liệu được hoàn trả.
        using var verify = new SqlCommand(
            "SELECT SoLuongTon FROM dbo.SanPham WHERE MaSanPham=@Id", connection);
        verify.Parameters.Add("@Id", SqlDbType.Int).Value = 1;
        int after = Convert.ToInt32(verify.ExecuteScalar());
        Console.WriteLine($"-> Tồn kho trước: {before}; sau rollback: {after}.");
        if (before != after)
            throw new InvalidOperationException("Kiểm chứng thất bại: tồn kho không trở về giá trị ban đầu.");
        Console.WriteLine("-> KIỂM CHỨNG ĐẠT: Sản phẩm 1 không bị trừ kho sau lỗi ở bước 2.");
    }
}
