using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    // Form quản lý nhóm hàng của siêu thị Đời Sống Xanh (Tự co giãn toàn màn hình, có phân trang)
    public partial class FormCategoryManagement : Form
    {
        private static readonly HttpClient _client = ApiClientService.Client;

        private readonly Button btnPrev = new() { Name = "btnPrev", Text = "<< Trang trước", AutoSize = true, Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand };
        private readonly Button btnNext = new() { Name = "btnNext", Text = "Trang sau >>", AutoSize = true, Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand };
        private readonly Label lblPaging = new() { Name = "lblPaging", AutoSize = true, Padding = new Padding(8), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

        private const int PageSize = 5;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private bool _busy;

        public FormCategoryManagement()
        {
            InitializeComponent();
            ConfigureLayout();
            ConfigureGrid();

            Load += async (_, _) => await LoadDataAsync(1);
            btnSearch.Click += async (_, _) => await LoadDataAsync(1);
            btnLoad.Click += async (_, _) =>
            {
                txtKeyword.Clear();
                ClearInputs();
                await LoadDataAsync(1);
            };
            btnPrev.Click += async (_, _) => await LoadDataAsync(_currentPage - 1);
            btnNext.Click += async (_, _) => await LoadDataAsync(_currentPage + 1);
            txtKeyword.KeyDown += async (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                await LoadDataAsync(1);
            };

            ApplyPermissions();
        }

        // Bố trí giao diện co giãn toàn màn hình theo Shell y hệt trang sản phẩm
        private void ConfigureLayout()
        {
            SuspendLayout();
            AutoScroll = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(12)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));

            // 1. Thanh tìm kiếm
            var search = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

            txtKeyword.Dock = DockStyle.Fill;
            btnSearch.Dock = btnLoad.Dock = DockStyle.Fill;
            search.Controls.Add(txtKeyword, 0, 0);
            search.Controls.Add(btnSearch, 1, 0);
            search.Controls.Add(btnLoad, 2, 0);

            // 2. Danh sách bảng nhóm hàng
            grpCategoryList.Dock = DockStyle.Fill;
            grpCategoryList.Padding = new Padding(8, 22, 8, 8);
            dgvCategories.Dock = DockStyle.Fill;

            // 3. Thanh phân trang
            var pager = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            pager.Controls.AddRange([btnPrev, lblPaging, btnNext]);

            // 4. Form thông tin và nút tác vụ
            grpCategoryInfo.Dock = DockStyle.Fill;
            grpCategoryInfo.Controls.Clear();

            var editor = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(6)
            };
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            editor.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            // Hàng 0: Mã ID và Tên nhóm hàng
            lblId.Dock = txtId.Dock = lblCategoryName.Dock = txtCategoryName.Dock = DockStyle.Fill;
            lblId.Margin = lblCategoryName.Margin = new Padding(2, 7, 2, 2);
            editor.Controls.Add(lblId, 0, 0);
            editor.Controls.Add(txtId, 1, 0);
            editor.Controls.Add(lblCategoryName, 2, 0);
            editor.Controls.Add(txtCategoryName, 3, 0);

            // Hàng 1: Mô tả nhóm hàng
            lblDescription.Dock = txtDescription.Dock = DockStyle.Fill;
            lblDescription.Margin = new Padding(2, 7, 2, 2);
            editor.Controls.Add(lblDescription, 0, 1);
            editor.Controls.Add(txtDescription, 1, 1);
            editor.SetColumnSpan(txtDescription, 3);

            // Hàng 2: Các nút Thêm, Sửa, Xóa
            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            foreach (var button in new[] { btnAdd, btnUpdate, btnDelete })
            {
                button.AutoSize = true;
                button.Margin = new Padding(6, 6, 6, 6);
                button.Padding = new Padding(10, 4, 10, 4);
                button.Cursor = Cursors.Hand;
                actions.Controls.Add(button);
            }
            editor.Controls.Add(actions, 0, 2);
            editor.SetColumnSpan(actions, 4);
            grpCategoryInfo.Controls.Add(editor);

            Controls.Clear();
            layout.Controls.Add(search, 0, 0);
            layout.Controls.Add(grpCategoryList, 0, 1);
            layout.Controls.Add(pager, 0, 2);
            layout.Controls.Add(grpCategoryInfo, 0, 3);
            Controls.Add(layout);

            ResumeLayout(true);
        }

        private void ConfigureGrid()
        {
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.ReadOnly = true;
            dgvCategories.MultiSelect = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCategories.ColumnHeadersHeight = 40;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCategories.DefaultCellStyle.Font = new Font("Segoe UI", 10.5f);
            dgvCategories.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCategories.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
            dgvCategories.RowTemplate.Height = 44;

            dgvCategories.Resize += (_, _) => FormProductManagement.AdjustRowHeights(dgvCategories, PageSize);
            dgvCategories.CellClick += dgvCategories_CellClick;
        }

        // Tải danh sách nhóm hàng có phân trang từ API
        public async Task LoadDataAsync(int page = 1)
        {
            if (_busy) return;
            _busy = true;
            UpdatePagingButtons();

            try
            {
                string keyword = Uri.EscapeDataString(txtKeyword.Text.Trim());
                using var response = await SendRequestAsync(
                    HttpMethod.Get,
                    $"categories/paged?keyword={keyword}&pageNumber={page}&pageSize={PageSize}");

                if (!CheckResponse(response))
                    return;

                var result = await response.Content.ReadFromJsonAsync<PagedViewResult<CategoryDto>>();
                if (result == null || IsDisposed) return;

                if (result.TotalPages > 0 && page > result.TotalPages)
                {
                    await LoadDataAsync(result.TotalPages);
                    return;
                }

                _currentPage = result.TotalItems == 0 ? 1 : result.CurrentPage;
                _totalPages = result.TotalPages;

                dgvCategories.DataSource = null;
                dgvCategories.AutoGenerateColumns = true;
                dgvCategories.Columns.Clear();
                dgvCategories.DataSource = result.Items ?? new List<CategoryDto>();

                FormatDataGridView();
                FormProductManagement.AdjustRowHeights(dgvCategories, PageSize);
                dgvCategories.ClearSelection();
                ClearInputs();

                lblPaging.Text = $"Trang {(_totalPages == 0 ? 0 : _currentPage)} / {_totalPages} (Tổng: {result.TotalItems} nhóm hàng)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách nhóm hàng:\n" + ApiClientService.FriendlyError(ex),
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _busy = false;
                if (!IsDisposed) UpdatePagingButtons();
            }
        }

        private void UpdatePagingButtons()
        {
            btnPrev.Enabled = !_busy && _currentPage > 1;
            btnNext.Enabled = !_busy && _currentPage < _totalPages;
            btnSearch.Enabled = btnLoad.Enabled = txtKeyword.Enabled = !_busy;
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = !_busy && CanManageRole();
        }

        private bool CanManageRole() =>
            string.Equals(SessionManager.CurrentRole, "Admin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(SessionManager.CurrentRole, "Warehouse", StringComparison.OrdinalIgnoreCase);

        private void FormatDataGridView()
        {
            if (dgvCategories.Columns.Count == 0) return;

            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvCategories.Columns["MaNhomHang"] != null)
            {
                var col = dgvCategories.Columns["MaNhomHang"]!;
                col.HeaderText = "Mã ID";
                col.FillWeight = 20;
            }

            if (dgvCategories.Columns["TenNhomHang"] != null)
            {
                var col = dgvCategories.Columns["TenNhomHang"]!;
                col.HeaderText = "Tên Nhóm hàng";
                col.FillWeight = 35;
            }

            if (dgvCategories.Columns["MoTa"] != null)
            {
                var col = dgvCategories.Columns["MoTa"]!;
                col.HeaderText = "Mô Tả";
                col.FillWeight = 45;
            }
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);
            if (!string.IsNullOrWhiteSpace(SessionManager.JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }
            return request;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string endpoint, object? data = null)
        {
            using var request = CreateRequest(method, endpoint);
            if (data != null) request.Content = JsonContent.Create(data);
            return await _client.SendAsync(request);
        }

        private bool CheckResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;

            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Phiên đăng nhập không hợp lệ hoặc đã hết hạn!",
                HttpStatusCode.Forbidden => "Tài khoản không có quyền thực hiện chức năng này!",
                HttpStatusCode.NotFound => "Không tìm thấy nhóm hàng!",
                HttpStatusCode.Conflict => "Dữ liệu đang được sử dụng hoặc bị xung đột!",
                _ => $"Yêu cầu thất bại! Mã HTTP: {(int)response.StatusCode}"
            };

            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void ApplyPermissions()
        {
            bool canManage = CanManageRole();
            btnAdd.Enabled = canManage;
            btnUpdate.Enabled = canManage;
            btnDelete.Enabled = canManage;
        }

        private void dgvCategories_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCategories.Rows.Count) return;
            var row = dgvCategories.Rows[e.RowIndex];
            txtId.Text = row.Cells["MaNhomHang"].Value?.ToString() ?? string.Empty;
            txtCategoryName.Text = row.Cells["TenNhomHang"].Value?.ToString() ?? string.Empty;
            txtDescription.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CanManageCategories()) return;

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            try
            {
                var newCategory = new
                {
                    TenNhomHang = txtCategoryName.Text.Trim(),
                    MoTa = txtDescription.Text.Trim()
                };

                using var response = await SendRequestAsync(HttpMethod.Post, "categories", newCategory);
                if (!CheckResponse(response)) return;

                MessageBox.Show("Thêm nhóm hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhóm hàng:\n" + ApiClientService.FriendlyError(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CanManageCategories()) return;

            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var updatedCategory = new
                {
                    TenNhomHang = txtCategoryName.Text.Trim(),
                    MoTa = txtDescription.Text.Trim()
                };

                using var response = await SendRequestAsync(HttpMethod.Put, $"categories/{id}", updatedCategory);
                if (!CheckResponse(response)) return;

                MessageBox.Show("Cập nhật nhóm hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật nhóm hàng:\n" + ApiClientService.FriendlyError(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CanManageCategories()) return;

            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhóm hàng có mã {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using var response = await SendRequestAsync(HttpMethod.Delete, $"categories/{id}");
                if (!CheckResponse(response)) return;

                MessageBox.Show("Xóa nhóm hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa nhóm hàng:\n" + ApiClientService.FriendlyError(ex) + "\n\nNếu nhóm hàng đang có sản phẩm, CSDL không cho phép xóa.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e) => await LoadDataAsync(1);
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            ClearInputs();
            await LoadDataAsync(1);
        }

        private bool CanManageCategories()
        {
            if (CanManageRole()) return true;
            MessageBox.Show("Chỉ Admin hoặc Warehouse được quản lý nhóm hàng!", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        private async void FormCategoryManagement_Load(object? sender, EventArgs e) => await LoadDataAsync(1);

        private void grpCategoryList_Enter(object sender, EventArgs e) { }
    }

    public class CategoryDto
    {
        public int MaNhomHang { get; set; }
        public string TenNhomHang { get; set; } = string.Empty;
        public string? MoTa { get; set; }
    }
}
