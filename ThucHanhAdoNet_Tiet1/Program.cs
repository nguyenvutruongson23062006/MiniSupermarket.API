using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ThucHanhAdoNet_Tiet1;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            string connectionString = GetConnectionString();
            if (args.Length > 0)
            {
                RunExercise(args[0], connectionString, args.Skip(1).ToArray());
                return 0;
            }

            while (true)
            {
                Console.WriteLine("\n=== TIẾT 1: CÁC THÀNH PHẦN CỐT LÕI ADO.NET ===");
                Console.WriteLine("1. Đếm sản phẩm bằng ExecuteScalar()");
                Console.WriteLine("2. Cập nhật tồn kho bằng ExecuteNonQuery() và SqlParameter");
                Console.WriteLine("3. Đọc danh sách bằng SqlDataReader");
                Console.WriteLine("4. Giải thích các thành phần ADO.NET và ACID");
                Console.WriteLine("5. Bài 2.2: Đếm sản phẩm theo nhóm bằng DataTable");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn bài tập: ");
                string? choice = Console.ReadLine();
                if (choice is null or "0") return 0;

                try
                {
                    RunExercise(choice, connectionString, []);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LỖI]: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[LỖI]: {ex.Message}");
            return 1;
        }
    }

    private static string GetConnectionString()
    {
        // Dùng chung cấu hình SQL Server với API, không tạo database khác.
        string? configured = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(configured)) return configured;

        using var settings = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.json")));
        return settings.RootElement.GetProperty("ConnectionStrings")
            .GetProperty("DefaultConnection").GetString()
            ?? throw new InvalidOperationException("Thiếu chuỗi kết nối SQL Server.");
    }

    private static void RunExercise(string choice, string connectionString, string[] arguments)
    {
        switch (choice)
        {
            case "1": CountProducts(connectionString); break;
            case "2":
                int productId = ReadNumber(arguments, 0, "Mã sản phẩm", 1, 1);
                int stock = ReadNumber(arguments, 1, "Tồn kho mới", 250, 0);
                UpdateStock(connectionString, productId, stock);
                break;
            case "3": ReadProducts(connectionString); break;
            case "4": ExplainConcepts(); break;
            case "5": CountByCategory(connectionString); break;
            default: throw new ArgumentException("Chọn bài từ 1 đến 5.");
        }
    }

    private static int ReadNumber(string[] arguments, int index, string label, int defaultValue, int minimum)
    {
        string? value;
        if (index < arguments.Length) value = arguments[index];
        else
        {
            Console.Write($"{label} (Enter = {defaultValue}): ");
            value = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
        }

        if (!int.TryParse(value, out int result) || result < minimum)
            throw new ArgumentException($"{label} phải là số nguyên từ {minimum} trở lên.");
        return result;
    }

    // Bài 1: ExecuteScalar trả về ô ở dòng đầu, cột đầu.
    private static void CountProducts(string connectionString)
    {
        Console.WriteLine("\n=== BÀI TẬP 1: ĐẾM TỔNG SỐ LƯỢNG MẶT HÀNG ===");
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand("SELECT COUNT(*) FROM dbo.SanPham", connection);
        connection.Open();
        int total = Convert.ToInt32(command.ExecuteScalar());
        Console.WriteLine($"-> Tổng số mặt hàng hiện có: {total} sản phẩm.");
    }

    // Bài 2: UPDATE có tham số, lưu tồn kho mới theo bài giảng.
    private static void UpdateStock(string connectionString, int productId, int stock)
    {
        Console.WriteLine("\n=== BÀI TẬP 2: CẬP NHẬT TỒN KHO AN TOÀN VỚI SQLPARAMETER ===");
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(
            "UPDATE dbo.SanPham SET SoLuongTon=@Stock WHERE MaSanPham=@Id", connection);
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = productId;
        connection.Open();
        int affected = command.ExecuteNonQuery();
        if (affected == 0)
            Console.WriteLine($"-> Không tìm thấy sản phẩm ID = {productId}.");
        else
            Console.WriteLine($"-> Đã lưu tồn kho sản phẩm ID = {productId} thành {stock}. Số dòng bị ảnh hưởng: {affected}.");
    }

    // Bài 3: reader đọc lần lượt từng dòng trong lúc kết nối còn mở.
    private static void ReadProducts(string connectionString)
    {
        Console.WriteLine("\n=== BÀI TẬP 3: DUYỆT DANH SÁCH BẰNG SQLDATAREADER ===");
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand("""
            SELECT MaSanPham, MaVach, TenSanPham, GiaBan, SoLuongTon
            FROM dbo.SanPham ORDER BY MaSanPham
            """, connection);
        connection.Open();
        using var reader = command.ExecuteReader();
        Console.WriteLine($"{"MÃ SP",-6} | {"MÃ VẠCH",-14} | {"TÊN SẢN PHẨM",-40} | {"ĐƠN GIÁ",12} | {"TỒN KHO",8}");
        int count = 0;
        while (reader.Read())
        {
            count++;
            Console.WriteLine($"{reader.GetInt32(0),-6} | {reader.GetString(1),-14} | {reader.GetString(2),-40} | {reader.GetDecimal(3),12:N0} | {reader.GetInt32(4),8}");
        }
        Console.WriteLine($"-> Tổng cộng đã đọc: {count} mặt hàng.");
    }

    private static void CountByCategory(string connectionString)
    {
        Console.WriteLine("=== BÀI 2.2: ĐẾM SẢN PHẨM THEO NHÓM BẰNG DATATABLE ===");
        using var table = new DataTable();
        using (var connection = new SqlConnection(connectionString))
        using (var command = new SqlCommand("""
            SELECT MaNhomHang, COUNT(*) AS Total FROM dbo.SanPham
            GROUP BY MaNhomHang ORDER BY MaNhomHang
            """, connection))
        using (var adapter = new SqlDataAdapter(command))
        {
            adapter.Fill(table); // Adapter tự mở và đóng connection đang đóng.
        }
        // Kết nối đã Dispose, vẫn đọc được DataTable trong bộ nhớ.
        foreach (DataRow row in table.Rows)
            Console.WriteLine($"Nhóm {row["MaNhomHang"]}: {row["Total"]} sản phẩm.");
    }

    private static void ExplainConcepts()
    {
        Console.WriteLine("=== KIẾN THỨC ADO.NET VÀ ACID ===");
        Console.WriteLine("SqlConnection: kết nối tới SQL Server. using tự Dispose cả khi có lỗi, trả kết nối về pool.");
        Console.WriteLine("SqlCommand: chứa câu SQL hoặc tên Stored Procedure và các tham số cần thực thi.");
        Console.WriteLine("SqlParameter: gửi giá trị riêng với câu lệnh, không ghép đầu vào thành SQL, tránh SQL Injection.");
        Console.WriteLine("ExecuteScalar: lấy ô đầu tiên; ví dụ COUNT sản phẩm hoặc OUTPUT mã hóa đơn vừa tạo.");
        Console.WriteLine("ExecuteNonQuery: thực thi INSERT/UPDATE/DELETE, trả số dòng tác động khi SQL có báo số dòng.");
        Console.WriteLine("ExecuteReader: đọc tuần tự nhiều dòng, chỉ đọc và cần giữ kết nối mở.");
        Console.WriteLine("DataTable: giữ dữ liệu trong RAM. SqlDataAdapter.Fill tự mở/đóng kết nối nếu ban đầu đang đóng.");
        Console.WriteLine("Stored Procedure: thủ tục trên SQL Server; SqlCommand dùng CommandType.StoredProcedure.");
        Console.WriteLine("Atomicity: tạo đơn, lưu chi tiết, trừ kho, cộng điểm cùng thành công hoặc cùng hoàn tác.");
        Console.WriteLine("Consistency: giao dịch giữ đúng ràng buộc, không trừ tồn kho xuống âm.");
        Console.WriteLine("Isolation: khóa tồn kho trong giao dịch để các quầy không cùng bán vượt số hàng còn lại.");
        Console.WriteLine("Durability: sau Commit, thay đổi được lưu bền vững trong database.");
        Console.WriteLine("BeginTransaction bắt đầu; mọi command dùng cùng connection và transaction.");
        Console.WriteLine("Commit xác nhận lưu khi đủ bước; Rollback hủy thay đổi nếu có bước lỗi, không gọi hai lần.");
    }
}
