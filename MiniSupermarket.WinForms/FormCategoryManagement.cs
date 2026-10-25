
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    // Form quản lý nhóm hàng của siêu thị Đời Sống Xanh
    public partial class FormCategoryManagement : Form
    {
        // Địa chỉ Web API của hệ thống
        private static readonly HttpClient _client = ApiClientService.Client;

        // Hàm khởi tạo Form quản lý nhóm hàng
        public FormCategoryManagement()
        {
            InitializeComponent();

            // Không cho phép tự thêm dòng vào DataGridView
            dgvCategories.AllowUserToAddRows = false;

            // Chỉ cho phép chọn nguyên dòng
            dgvCategories.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCategories.MultiSelect = false;
            dgvCategories.ReadOnly = true;

            // Cho DataGridView giãn theo kích thước GroupBox
            dgvCategories.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // Thiết lập quyền sử dụng các nút
            ApplyPermissions();
        }

        // Hàm xử lý khi Form được mở
        private async void FormCategoryManagement_Load(
            object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        // Hàm tạo HTTP request có gắn JWT Token
        private HttpRequestMessage CreateRequest(
            HttpMethod method,
            string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!string.IsNullOrWhiteSpace(SessionManager.JwtToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        SessionManager.JwtToken);
            }

            return request;
        }

        // Hàm gửi HTTP request đến Web API
        private async Task<HttpResponseMessage> SendRequestAsync(
            HttpMethod method,
            string endpoint,
            object? data = null)
        {
            using var request = CreateRequest(method, endpoint);

            // Đính kèm dữ liệu JSON khi thêm hoặc cập nhật
            if (data != null)
            {
                request.Content = JsonContent.Create(data);
            }

            return await _client.SendAsync(request);
        }

        // Hàm kiểm tra và thông báo lỗi từ Web API
        private bool CheckResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show(
                    "Phiên đăng nhập không hợp lệ hoặc đã hết hạn!",
                    "Lỗi xác thực",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                MessageBox.Show(
                    "Tài khoản không có quyền thực hiện chức năng này!",
                    "Không đủ quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show(
                    "Không tìm thấy nhóm hàng!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show(
                    "Dữ liệu đang được sử dụng hoặc bị xung đột!",
                    "Không thể thực hiện",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(
                    $"Yêu cầu thất bại! Mã HTTP: {(int)response.StatusCode}",
                    "Lỗi Web API",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return false;
        }

        // Hàm phân quyền các nút chức năng
        private void ApplyPermissions()
        {
            bool canManage = string.Equals(
                SessionManager.CurrentRole,
                "Admin",
                StringComparison.OrdinalIgnoreCase)
                || string.Equals(
                    SessionManager.CurrentRole,
                    "Warehouse",
                    StringComparison.OrdinalIgnoreCase);

            btnAdd.Enabled = canManage;
            btnUpdate.Enabled = canManage;
            btnDelete.Enabled = canManage;
        }

        // Hàm tải danh sách nhóm hàng từ SQL Server qua API
        private async Task LoadDataAsync()
        {
            try
            {
                using var response = await SendRequestAsync(
                    HttpMethod.Get,
                    "categories");

                if (!CheckResponse(response))
                    return;

                var categories =
                    await response.Content.ReadFromJsonAsync<
                        List<CategoryDto>>();

                // Xóa liên kết dữ liệu cũ
                dgvCategories.DataSource = null;

                // Tạo lại các cột theo DTO hiện tại
                dgvCategories.AutoGenerateColumns = true;
                dgvCategories.Columns.Clear();

                // Gán dữ liệu mới
                dgvCategories.DataSource =
                    categories ?? new List<CategoryDto>();

                // Định dạng DataGridView
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách nhóm hàng:\n" + ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // ĐỊNH DẠNG DATAGRIDVIEW - ĐÃ SỬA GIÃN HẾT CHIỀU NGANG
        // =========================================================
        private void FormatDataGridView()
        {
            if (dgvCategories.Columns.Count == 0)
                return;

            // Tự động phân bổ chiều rộng cho các cột
            dgvCategories.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // Cột mã nhóm hàng
            if (dgvCategories.Columns["MaNhomHang"] != null)
            {
                var col = dgvCategories.Columns["MaNhomHang"]!;

                col.HeaderText = "Mã ID";
                col.MinimumWidth = 65;
                col.FillWeight = 12;
                col.DisplayIndex = 0;
            }

            // Cột tên nhóm hàng
            if (dgvCategories.Columns["TenNhomHang"] != null)
            {
                var col = dgvCategories.Columns["TenNhomHang"]!;

                col.HeaderText = "Tên Nhóm hàng";
                col.MinimumWidth = 180;
                col.FillWeight = 38;
                col.DisplayIndex = 1;
            }

            // Cột mô tả nhóm hàng
            if (dgvCategories.Columns["MoTa"] != null)
            {
                var col = dgvCategories.Columns["MoTa"]!;

                col.HeaderText = "Mô Tả";
                col.MinimumWidth = 220;
                col.FillWeight = 50;
                col.DisplayIndex = 2;
            }

            // Thiết lập DataGridView
            dgvCategories.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCategories.MultiSelect = false;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.ReadOnly = true;

            dgvCategories.RowHeadersWidth = 30;
            dgvCategories.AllowUserToResizeRows = false;

            // Cột tự giãn khi DataGridView thay đổi kích thước
            dgvCategories.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Hàm lấy thông tin nhóm hàng khi chọn một dòng
        private void dgvCategories_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.RowIndex >= dgvCategories.Rows.Count)
                return;

            var row = dgvCategories.Rows[e.RowIndex];

            // Lấy mã nhóm hàng
            txtId.Text =
                row.Cells["MaNhomHang"].Value?.ToString()
                ?? string.Empty;

            // Lấy tên nhóm hàng
            txtCategoryName.Text =
                row.Cells["TenNhomHang"].Value?.ToString()
                ?? string.Empty;

            // Lấy mô tả nhóm hàng
            txtDescription.Text =
                row.Cells["MoTa"].Value?.ToString()
                ?? string.Empty;
        }

        // Hàm thêm nhóm hàng mới
        private async void btnAdd_Click(
            object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin hoặc Warehouse
            if (!CanManageCategories())
                return;

            // Kiểm tra tên nhóm hàng
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên nhóm hàng!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCategoryName.Focus();
                return;
            }

            try
            {
                // Tạo dữ liệu nhóm hàng mới
                var newCategory = new
                {
                    TenNhomHang = txtCategoryName.Text.Trim(),
                    MoTa = txtDescription.Text.Trim()
                };

                // Gửi POST đến API
                using var response = await SendRequestAsync(
                    HttpMethod.Post,
                    "categories",
                    newCategory);

                if (!CheckResponse(response))
                    return;

                MessageBox.Show(
                    "Thêm nhóm hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Tải lại dữ liệu
                await LoadDataAsync();

                // Xóa nội dung các ô nhập
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thêm nhóm hàng:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Hàm cập nhật thông tin nhóm hàng
        private async void btnUpdate_Click(
            object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin hoặc Warehouse
            if (!CanManageCategories())
                return;

            // Kiểm tra mã nhóm hàng
            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần cập nhật!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Kiểm tra tên nhóm hàng
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên nhóm hàng!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                // Dữ liệu cập nhật
                var updatedCategory = new
                {
                    MaNhomHang = id,
                    TenNhomHang = txtCategoryName.Text.Trim(),
                    MoTa = txtDescription.Text.Trim()
                };

                // Gửi PUT đến API
                using var response = await SendRequestAsync(
                    HttpMethod.Put,
                    $"categories/{id}",
                    updatedCategory);

                if (!CheckResponse(response))
                    return;

                MessageBox.Show(
                    "Cập nhật nhóm hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi cập nhật nhóm hàng:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Hàm xóa nhóm hàng
        private async void btnDelete_Click(
            object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin hoặc Warehouse
            if (!CanManageCategories())
                return;

            // Kiểm tra mã nhóm hàng
            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần xóa!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Yêu cầu xác nhận trước khi xóa
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhóm hàng có mã {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                // Gửi DELETE đến API
                using var response = await SendRequestAsync(
                    HttpMethod.Delete,
                    $"categories/{id}");

                if (!CheckResponse(response))
                    return;

                MessageBox.Show(
                    "Xóa nhóm hàng thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa nhóm hàng:\n" + ex.Message +
                    "\n\nNếu nhóm hàng đang có sản phẩm, " +
                    "SQL Server có thể không cho phép xóa.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Hàm tìm kiếm nhóm hàng theo tên
        private async void btnSearch_Click(
            object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();

            // Nếu từ khóa trống thì tải toàn bộ danh sách
            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadDataAsync();
                return;
            }

            try
            {
                // Mã hóa từ khóa để gửi qua URL
                string encodedKeyword =
                    Uri.EscapeDataString(keyword);

                // Gửi GET tìm kiếm có JWT
                using var response = await SendRequestAsync(
                    HttpMethod.Get,
                    $"categories/search?keyword={encodedKeyword}");

                if (!CheckResponse(response))
                    return;

                // Đọc kết quả tìm kiếm
                var result =
                    await response.Content.ReadFromJsonAsync<
                        List<CategoryDto>>();

                // Cập nhật bảng
                dgvCategories.DataSource = null;
                dgvCategories.AutoGenerateColumns = true;
                dgvCategories.Columns.Clear();

                dgvCategories.DataSource =
                    result ?? new List<CategoryDto>();

                FormatDataGridView();
                ClearInputs();

                // Thông báo nếu không tìm thấy
                if (result == null || result.Count == 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy nhóm hàng phù hợp!",
                        "Kết quả tìm kiếm",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tìm kiếm nhóm hàng:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Hàm tải lại toàn bộ danh sách nhóm hàng
        private async void btnLoad_Click(
            object sender, EventArgs e)
        {
            // Xóa từ khóa tìm kiếm
            txtKeyword.Clear();

            // Xóa dữ liệu các ô nhập
            ClearInputs();

            // Tải lại danh sách
            await LoadDataAsync();
        }

        // Hàm kiểm tra quyền quản lý nhóm hàng
        private bool CanManageCategories()
        {
            bool allowed = string.Equals(
                    SessionManager.CurrentRole,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase)
                || string.Equals(
                    SessionManager.CurrentRole,
                    "Warehouse",
                    StringComparison.OrdinalIgnoreCase);

            if (allowed)
            {
                return true;
            }

            MessageBox.Show(
                "Chỉ Admin hoặc Warehouse được quản lý nhóm hàng!",
                "Không đủ quyền",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        // Hàm xóa nội dung các ô nhập
        private void ClearInputs()
        {
            txtId.Clear();
            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        // Giữ lại sự kiện đã tạo trong Designer
        private void grpCategoryList_Enter(
            object sender, EventArgs e)
        {
        }
    }

    // DTO nhận dữ liệu nhóm hàng từ Web API
    public class CategoryDto
    {
        // Mã nhóm hàng
        public int MaNhomHang { get; set; }

        // Tên nhóm hàng
        public string TenNhomHang { get; set; } = string.Empty;

        // Mô tả nhóm hàng
        public string? MoTa { get; set; }
    }
}
