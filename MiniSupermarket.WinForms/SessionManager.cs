
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSupermarket.WinForms
{
    // Quản lý thông tin phiên đăng nhập của người dùng
    public static class SessionManager
    {
        // Lưu JWT Token sau khi đăng nhập
        public static string JwtToken { get; set; } = string.Empty;

        // Lưu vai trò: Admin, Cashier hoặc Warehouse
        public static string CurrentRole { get; set; } = string.Empty;

        // Lưu tên đăng nhập của người dùng
        public static string CurrentUsername { get; set; } = string.Empty;
    }

    // Lớp hỗ trợ gọi API của hệ thống siêu thị
    public static class ApiClientService
    {
        // Khởi tạo HttpClient kết nối đến Web API
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7163/api/")
        };

        // Dùng chung một HttpClient cho toàn bộ các Form nghiệp vụ.
        public static HttpClient Client => _client;

        // Tạo request có JWT để tránh lặp lại mã gắn Bearer Token.
        public static HttpRequestMessage CreateRequest(
            HttpMethod method,
            string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!string.IsNullOrWhiteSpace(
                SessionManager.JwtToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        SessionManager.JwtToken);
            }

            return request;
        }

        // Hàm gọi API đăng nhập và lưu thông tin phiên làm việc
        public static async Task<bool> LoginAsync(
            string username,
            string password)
        {
            // Tạo dữ liệu gửi đến API
            var loginObj = new
            {
                Username = username,
                Password = password
            };

            // Gửi yêu cầu đăng nhập
            var response = await _client.PostAsJsonAsync(
                "auth/login",
                loginObj);

            if (response.IsSuccessStatusCode)
            {
                // Đọc kết quả JSON từ API
                var jsonString =
                    await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonString);

                // Lưu JWT Token
                SessionManager.JwtToken =
                    doc.RootElement.GetProperty("token").GetString()
                    ?? string.Empty;

                // Lưu vai trò người dùng
                SessionManager.CurrentRole =
                    doc.RootElement.GetProperty("role").GetString()
                    ?? string.Empty;

                // Lưu tên đăng nhập từ API
                SessionManager.CurrentUsername =
                    doc.RootElement.GetProperty("username").GetString()
                    ?? username;

                return true;
            }

            return false;
        }

        // Hàm gọi API lấy dữ liệu có kèm JWT Bearer Token
        public static async Task<string> GetDataWithTokenAsync(
            string endpoint)
        {
            // Đính kèm Token vào yêu cầu
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    SessionManager.JwtToken);

            // Gửi yêu cầu GET đến API
            var response = await _client.GetAsync(endpoint);

            // Trả về dữ liệu nếu thành công
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            // Xử lý khi chưa đăng nhập hoặc Token hết hạn
            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception(
                    "Phiên làm việc hết hạn hoặc chưa đăng nhập!");
            }

            // Xử lý khi không đủ quyền truy cập
            if (response.StatusCode ==
                System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception(
                    "Bạn không có quyền truy cập chức năng này!");
            }

            // Xử lý các lỗi khác
            throw new Exception(
                "Lỗi khi gọi dữ liệu từ Server.");
        }
    }
}
