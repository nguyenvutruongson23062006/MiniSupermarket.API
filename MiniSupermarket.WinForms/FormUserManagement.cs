using System.Net;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormUserManagement : Form
    {
        private int? _selectedUserId;

        public FormUserManagement()
        {
            InitializeComponent();
            ConfigureGrid();

            cboRole.Items.AddRange(
            [
                "Admin",
                "Cashier",
                "Warehouse"
            ]);
            cboRole.SelectedItem = "Cashier";

            Load += async (_, _) => await LoadUsersAsync();
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;
            btnAddUser.Click +=
                async (_, _) => await AddUserAsync();
            btnResetPassword.Click +=
                async (_, _) => await ResetPasswordAsync();
            btnToggleLock.Click +=
                async (_, _) => await ToggleLockAsync();
            btnLoad.Click +=
                async (_, _) => await LoadUsersAsync();
            btnClear.Click += (_, _) => ClearInputs();
        }

        private void ConfigureGrid()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.MultiSelect = false;
            dgvUsers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            AddColumn("MaNguoiDung", "Mã", 55);
            AddColumn("TenDangNhap", "Tên đăng nhập", 120);
            AddColumn("HoTen", "Họ và tên", 180);
            AddColumn("Email", "Email", 190);
            AddColumn("VaiTro", "Vai trò", 100);
            AddColumn("TrangThai", "Trạng thái", 110);
        }

        private void AddColumn(
            string property,
            string header,
            int width)
        {
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = header,
                Width = width,
                AutoSizeMode = property == "HoTen"
                    ? DataGridViewAutoSizeColumnMode.Fill
                    : DataGridViewAutoSizeColumnMode.None
            });
        }

        private bool EnsureAdmin()
        {
            if (string.Equals(
                SessionManager.CurrentRole,
                "Admin",
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            MessageBox.Show(
                "Chỉ Admin được quản trị tài khoản.",
                "Không đủ quyền",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop);
            return false;
        }

        private async Task LoadUsersAsync()
        {
            if (!EnsureAdmin())
                return;

            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Get,
                    "users");
                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                var users = await response.Content
                    .ReadFromJsonAsync<List<UserManagementDto>>()
                    ?? new List<UserManagementDto>();

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;
                dgvUsers.ClearSelection();
                _selectedUserId = null;
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
        }

        private async Task AddUserAsync()
        {
            if (!EnsureAdmin())
                return;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                txtPassword.Text.Length < 6 ||
                cboRole.SelectedItem == null)
            {
                MessageBox.Show(
                    "Nhập đầy đủ tên đăng nhập, họ tên, vai trò " +
                    "và mật khẩu từ 6 ký tự.",
                    "Kiểm tra dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var data = new
            {
                TenDangNhap = txtUsername.Text.Trim(),
                MatKhau = txtPassword.Text,
                HoTen = txtFullName.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text)
                    ? null
                    : txtEmail.Text.Trim(),
                VaiTro = cboRole.SelectedItem.ToString()
            };

            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Post,
                    "users");
                request.Content = JsonContent.Create(data);

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                MessageBox.Show(
                    "Tạo tài khoản thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearInputs();
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
        }

        private async Task ResetPasswordAsync()
        {
            if (!EnsureAdmin() || !_selectedUserId.HasValue)
            {
                if (!_selectedUserId.HasValue)
                    MessageBox.Show("Vui lòng chọn tài khoản.");
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show(
                    "Mật khẩu mới phải có ít nhất 6 ký tự.");
                return;
            }

            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Put,
                    $"users/{_selectedUserId.Value}/reset-password");
                request.Content = JsonContent.Create(new
                {
                    MatKhau = txtPassword.Text
                });

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                MessageBox.Show("Đặt lại mật khẩu thành công!");
                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
        }

        private async Task ToggleLockAsync()
        {
            if (!EnsureAdmin() || !_selectedUserId.HasValue)
            {
                if (!_selectedUserId.HasValue)
                    MessageBox.Show("Vui lòng chọn tài khoản.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Bạn muốn khóa/mở khóa tài khoản đang chọn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Put,
                    $"users/{_selectedUserId.Value}/toggle-lock");
                request.Content = JsonContent.Create(new { });

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
        }

        private void dgvUsers_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem
                is not UserManagementDto user)
                return;

            _selectedUserId = user.MaNguoiDung;
            txtUsername.Text = user.TenDangNhap;
            txtFullName.Text = user.HoTen;
            txtEmail.Text = user.Email ?? string.Empty;
            cboRole.SelectedItem = user.VaiTro;
            txtPassword.Clear();
        }

        private void ClearInputs()
        {
            _selectedUserId = null;
            txtUsername.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cboRole.SelectedItem = "Cashier";
            dgvUsers.ClearSelection();
            txtUsername.Focus();
        }

        private static async Task<bool> EnsureSuccessAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string detail = await response.Content.ReadAsStringAsync();
            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized =>
                    "Phiên đăng nhập đã hết hạn.",
                HttpStatusCode.Forbidden =>
                    "Chỉ Admin được quản trị tài khoản.",
                HttpStatusCode.Conflict =>
                    "Tên đăng nhập đã tồn tại.",
                _ => string.IsNullOrWhiteSpace(detail)
                    ? $"API trả về mã {(int)response.StatusCode}."
                    : detail
            };

            MessageBox.Show(
                message,
                "Lỗi Web API",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        private static void ShowConnectionError(Exception ex)
        {
            MessageBox.Show(
                "Không thể kết nối Web API:\n" + ex.Message,
                "Lỗi kết nối",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public class UserManagementDto
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string VaiTro { get; set; } = string.Empty;
        public bool DangHoatDong { get; set; }
        public string TrangThai => DangHoatDong
            ? "Đang hoạt động"
            : "Đã khóa";
    }
}
