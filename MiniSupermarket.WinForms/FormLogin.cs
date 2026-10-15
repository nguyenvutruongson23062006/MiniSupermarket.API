
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        // Khởi tạo HttpClient kết nối đến Web API
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7163/api/")
        };

        // Hàm khởi tạo Form đăng nhập
        public FormLogin()
        {
            InitializeComponent();
        }

        // Hàm xử lý khi nhấn nút Đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Lấy thông tin người dùng nhập
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            // Kiểm tra thông tin đăng nhập
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Ngăn nhấn nút đăng nhập nhiều lần
            btnLogin.Enabled = false;

            try
            {
                // Tạo dữ liệu gửi đến API đăng nhập
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                // Gửi yêu cầu đến AuthController
                var response = await _client.PostAsJsonAsync(
                    "auth/login",
                    loginData);

                // Kiểm tra kết quả đăng nhập
                if (response.IsSuccessStatusCode)
                {
                    // Đọc JSON từ API
                    var jsonString =
                        await response.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(jsonString);

                    // Lưu JWT Token vào SessionManager
                    SessionManager.JwtToken =
                        doc.RootElement
                            .GetProperty("token")
                            .GetString() ?? string.Empty;

                    // Lưu vai trò của người dùng
                    SessionManager.CurrentRole =
                        doc.RootElement
                            .GetProperty("role")
                            .GetString() ?? string.Empty;

                    // Lưu tên đăng nhập của người dùng
                    SessionManager.CurrentUsername =
                        doc.RootElement
                            .GetProperty("username")
                            .GetString() ?? username;

                    // Hiển thị thông báo đăng nhập thành công
                    MessageBox.Show(
                        $"Đăng nhập thành công!\n" +
                        $"Tài khoản: {SessionManager.CurrentUsername}\n" +
                        $"Vai trò: {SessionManager.CurrentRole}",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Ẩn Form đăng nhập
                    this.Hide();

                    // Mở giao diện chính của siêu thị
                    using (FormMainShell mainForm = new FormMainShell())
                    {
                        mainForm.ShowDialog();
                    }

                    // Xóa thông tin phiên đăng nhập sau khi đóng MainShell
                    SessionManager.JwtToken = string.Empty;
                    SessionManager.CurrentRole = string.Empty;
                    SessionManager.CurrentUsername = string.Empty;

                    // Xóa mật khẩu và hiển thị lại FormLogin
                    txtPass.Clear();
                    this.Show();
                }
                else
                {
                    // Thông báo đăng nhập thất bại
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối Server
                MessageBox.Show(
                    "Lỗi kết nối đến Server: " + ex.Message,
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Đảm bảo FormLogin hiển thị nếu xảy ra lỗi
                this.Show();
            }
            finally
            {
                // Cho phép nhấn nút đăng nhập trở lại
                btnLogin.Enabled = true;
            }
        }

        // Sự kiện Enter của GroupBox đăng nhập
        private void groupBoxLogin_Enter(object sender, EventArgs e)
        {
        }

        // Sự kiện Enter thứ hai của GroupBox đăng nhập
        private void groupBoxLogin_Enter_1(object sender, EventArgs e)
        {
        }

        // Sự kiện khi FormLogin được mở
        private void FormLogin_Load(object sender, EventArgs e)
        {
        }
    }
}
