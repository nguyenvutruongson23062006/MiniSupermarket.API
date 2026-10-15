
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomerManagement : Form
    {
        // Địa chỉ Web API
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7163/api/")
        };

        public FormCustomerManagement()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.MultiSelect = false;

            dgvCustomers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCustomers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // Đăng ký sự kiện Load
            Load += FormCustomerManagement_Load;

            // Đăng ký sự kiện chọn dòng
            dgvCustomers.CellClick += dgvCustomers_CellClick;

            // Đăng ký các nút chức năng
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnSearch.Click += btnSearch_Click;
            btnLoad.Click += btnLoad_Click;

            // btnUpdate đã được Designer gắn với button3_Click
            // Không đăng ký lần nữa để tránh chạy hai lần

            ApplyPermissions();
        }

        // =========================================
        // 1. PHÂN QUYỀN
        // =========================================
        private void ApplyPermissions()
        {
            bool isAdmin = string.Equals(
                SessionManager.CurrentRole,
                "Admin",
                StringComparison.OrdinalIgnoreCase);

            bool isCashier = string.Equals(
                SessionManager.CurrentRole,
                "Cashier",
                StringComparison.OrdinalIgnoreCase);

            btnAdd.Enabled = isAdmin || isCashier;
            btnUpdate.Enabled = isAdmin || isCashier;
            btnDelete.Enabled = isAdmin;
        }

        private bool CanEdit()
        {
            bool allowed =
                string.Equals(
                    SessionManager.CurrentRole,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase)
                ||
                string.Equals(
                    SessionManager.CurrentRole,
                    "Cashier",
                    StringComparison.OrdinalIgnoreCase);

            if (!allowed)
            {
                MessageBox.Show(
                    "Bạn không có quyền thực hiện chức năng này!",
                    "Không đủ quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return allowed;
        }

        private bool IsAdmin()
        {
            bool isAdmin = string.Equals(
                SessionManager.CurrentRole,
                "Admin",
                StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
                MessageBox.Show(
                    "Chỉ Admin mới được xóa khách hàng!",
                    "Không đủ quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return isAdmin;
        }

        // =========================================
        // 2. TẠO REQUEST CÓ JWT
        // =========================================
        private HttpRequestMessage CreateRequest(
            HttpMethod method,
            string endpoint)
        {
            var request = new HttpRequestMessage(
                method,
                endpoint);

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

        private async Task<HttpResponseMessage> SendRequestAsync(
            HttpMethod method,
            string endpoint,
            object? data = null)
        {
            using var request = CreateRequest(
                method,
                endpoint);

            if (data != null)
            {
                request.Content = JsonContent.Create(data);
            }

            return await _client.SendAsync(request);
        }

        // =========================================
        // 3. KIỂM TRA PHẢN HỒI API
        // =========================================
        private async Task<bool> CheckResponseAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string message;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    message =
                        "Phiên đăng nhập không hợp lệ hoặc đã hết hạn!";
                    break;

                case HttpStatusCode.Forbidden:
                    message =
                        "Tài khoản không có quyền thực hiện chức năng này!";
                    break;

                case HttpStatusCode.NotFound:
                    message =
                        "Không tìm thấy khách hàng!";
                    break;

                case HttpStatusCode.Conflict:
                    message =
                        "Không thể xóa khách hàng vì đã có đơn hàng " +
                        "hoặc dữ liệu đang được sử dụng!";
                    break;

                case HttpStatusCode.BadRequest:
                    message =
                        "Dữ liệu gửi lên không hợp lệ!\n" +
                        await response.Content.ReadAsStringAsync();
                    break;

                default:
                    message =
                        $"Yêu cầu thất bại! HTTP {(int)response.StatusCode}";
                    break;
            }

            MessageBox.Show(
                message,
                "Thông báo Web API",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        // =========================================
        // 4. LOAD FORM
        // =========================================
        private async void FormCustomerManagement_Load(
            object? sender,
            EventArgs e)
        {
            await LoadDataAsync();
        }

        // =========================================
        // 5. TẢI DANH SÁCH KHÁCH HÀNG
        // =========================================
        private async Task LoadDataAsync()
        {
            try
            {
                using var response = await SendRequestAsync(
                    HttpMethod.Get,
                    "customers");

                if (!await CheckResponseAsync(response))
                    return;

                var customers =
                    await response.Content.ReadFromJsonAsync<
                        List<CustomerDto>>();

                BindCustomers(
                    customers ?? new List<CustomerDto>());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách khách hàng:\n" +
                    ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================
        // 6. HIỂN THỊ DATAGRIDVIEW
        // =========================================
        private void BindCustomers(
            List<CustomerDto> customers)
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.AutoGenerateColumns = true;
            dgvCustomers.Columns.Clear();

            dgvCustomers.DataSource = customers;

            FormatDataGridView();

            dgvCustomers.ClearSelection();
        }

        private void FormatDataGridView()
        {
            if (dgvCustomers.Columns.Count == 0)
                return;

            dgvCustomers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // Mã khách hàng
            if (dgvCustomers.Columns["MaKhachHang"] != null)
            {
                var col =
                    dgvCustomers.Columns["MaKhachHang"]!;

                col.HeaderText = "Mã KH";
                col.MinimumWidth = 65;
                col.FillWeight = 12;
                col.DisplayIndex = 0;
            }

            // Họ và tên
            if (dgvCustomers.Columns["HoTen"] != null)
            {
                var col = dgvCustomers.Columns["HoTen"]!;

                col.HeaderText = "Họ và tên";
                col.MinimumWidth = 120;
                col.FillWeight = 25;
                col.DisplayIndex = 1;
            }

            // Số điện thoại
            if (dgvCustomers.Columns["SoDienThoai"] != null)
            {
                var col =
                    dgvCustomers.Columns["SoDienThoai"]!;

                col.HeaderText = "Số điện thoại";
                col.MinimumWidth = 100;
                col.FillWeight = 18;
                col.DisplayIndex = 2;
            }

            // Email
            if (dgvCustomers.Columns["Email"] != null)
            {
                var col = dgvCustomers.Columns["Email"]!;

                col.HeaderText = "Email";
                col.MinimumWidth = 120;
                col.FillWeight = 20;
                col.DisplayIndex = 3;
            }

            // Địa chỉ
            if (dgvCustomers.Columns["DiaChi"] != null)
            {
                var col = dgvCustomers.Columns["DiaChi"]!;

                col.HeaderText = "Địa chỉ";
                col.MinimumWidth = 150;
                col.FillWeight = 25;
                col.DisplayIndex = 4;
            }

            dgvCustomers.ReadOnly = true;
            dgvCustomers.MultiSelect = false;

            dgvCustomers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.AllowUserToResizeRows = false;
        }

        // =========================================
        // 7. CHỌN DÒNG KHÁCH HÀNG
        // =========================================
        private void dgvCustomers_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.RowIndex >= dgvCustomers.Rows.Count)
                return;

            var row = dgvCustomers.Rows[e.RowIndex];

            txtCustomerId.Text =
                row.Cells["MaKhachHang"].Value?.ToString()
                ?? string.Empty;

            txtCustomerName.Text =
                row.Cells["HoTen"].Value?.ToString()
                ?? string.Empty;

            txtPhone.Text =
                row.Cells["SoDienThoai"].Value?.ToString()
                ?? string.Empty;

            txtEmail.Text =
                row.Cells["Email"].Value?.ToString()
                ?? string.Empty;

            txtAddress.Text =
                row.Cells["DiaChi"].Value?.ToString()
                ?? string.Empty;
        }

        // =========================================
        // 8. KIỂM TRA DỮ LIỆU NHẬP
        // =========================================
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(
                txtCustomerName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên khách hàng!");

                txtCustomerName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập số điện thoại!");

                txtPhone.Focus();
                return false;
            }

            string phone = txtPhone.Text.Trim();

            if (!Regex.IsMatch(phone, @"^\d{9,15}$"))
            {
                MessageBox.Show(
                    "Số điện thoại phải có từ 9 đến 15 chữ số!");

                txtPhone.Focus();
                return false;
            }

            string email = txtEmail.Text.Trim();

            if (!string.IsNullOrWhiteSpace(email) &&
                !Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show(
                    "Địa chỉ Email không hợp lệ!");

                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập địa chỉ khách hàng!");

                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private object GetCustomerData()
        {
            return new
            {
                HoTen = txtCustomerName.Text.Trim(),
                SoDienThoai = txtPhone.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(
                    txtEmail.Text)
                    ? null
                    : txtEmail.Text.Trim(),
                DiaChi = txtAddress.Text.Trim()
            };
        }

        // =========================================
        // 9. THÊM KHÁCH HÀNG
        // =========================================
        private async void btnAdd_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanEdit() || !ValidateInputs())
                return;

            try
            {
                using var response = await SendRequestAsync(
                    HttpMethod.Post,
                    "customers",
                    GetCustomerData());

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show(
                    "Thêm khách hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thêm khách hàng:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================
        // 10. CẬP NHẬT KHÁCH HÀNG
        // Giữ sự kiện button3_Click từ Designer
        // =========================================
        private async void button3_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanEdit())
                return;

            if (!int.TryParse(
                txtCustomerId.Text,
                out int id) || id <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn khách hàng cần cập nhật!");

                return;
            }

            if (!ValidateInputs())
                return;

            try
            {
                using var response = await SendRequestAsync(
                    HttpMethod.Put,
                    $"customers/{id}",
                    GetCustomerData());

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show(
                    "Cập nhật khách hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi cập nhật khách hàng:\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================
        // 11. XÓA KHÁCH HÀNG
        // =========================================
        private async void btnDelete_Click(
            object? sender,
            EventArgs e)
        {
            if (!IsAdmin())
                return;

            if (!int.TryParse(
                txtCustomerId.Text,
                out int id) || id <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn khách hàng cần xóa!");

                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng mã {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using var response = await SendRequestAsync(
                    HttpMethod.Delete,
                    $"customers/{id}");

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show(
                    "Xóa khách hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa khách hàng:\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================
        // 12. TÌM KIẾM KHÁCH HÀNG
        // =========================================
        private async void btnSearch_Click(
            object? sender,
            EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadDataAsync();
                return;
            }

            try
            {
                string encodedKeyword =
                    Uri.EscapeDataString(keyword);

                using var response = await SendRequestAsync(
                    HttpMethod.Get,
                    $"customers/search?keyword={encodedKeyword}");

                if (!await CheckResponseAsync(response))
                    return;

                var customers =
                    await response.Content.ReadFromJsonAsync<
                        List<CustomerDto>>();

                BindCustomers(
                    customers ?? new List<CustomerDto>());

                ClearInputs();

                if (customers == null ||
                    customers.Count == 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy khách hàng phù hợp!",
                        "Kết quả tìm kiếm",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tìm kiếm khách hàng:\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================
        // 13. TẢI LẠI DANH SÁCH
        // =========================================
        private async void btnLoad_Click(
            object? sender,
            EventArgs e)
        {
            txtKeyword.Clear();
            ClearInputs();

            await LoadDataAsync();
        }

        // =========================================
        // 14. XÓA CÁC Ô NHẬP
        // =========================================
        private void ClearInputs()
        {
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();

            dgvCustomers.ClearSelection();
        }
    }

    // DTO nhận dữ liệu từ CustomersController
    public class CustomerDto
    {
        public int MaKhachHang { get; set; }

        public string HoTen { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string DiaChi { get; set; } = string.Empty;
    }
}
