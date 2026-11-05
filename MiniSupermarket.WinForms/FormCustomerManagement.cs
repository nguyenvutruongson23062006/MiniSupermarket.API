using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace MiniSupermarket.WinForms
{
    // Form quản lý khách hàng của siêu thị Đời Sống Xanh (Tự co giãn toàn màn hình, có phân trang)
    public partial class FormCustomerManagement : Form
    {
        private static readonly HttpClient _client = ApiClientService.Client;

        private readonly Button btnPrev = new() { Name = "btnPrev", Text = "<< Trang trước", AutoSize = true, Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand };
        private readonly Button btnNext = new() { Name = "btnNext", Text = "Trang sau >>", AutoSize = true, Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand };
        private readonly Label lblPaging = new() { Name = "lblPaging", AutoSize = true, Padding = new Padding(8), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

        private const int PageSize = 5;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private bool _busy;

        public FormCustomerManagement()
        {
            InitializeComponent();
            ConfigureLayout();
            ConfigureGrid();

            Load += async (_, _) => await LoadDataAsync(1);
            dgvCustomers.CellClick += dgvCustomers_CellClick;

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += button3_Click;
            btnDelete.Click += btnDelete_Click;
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

        // Bố trí giao diện co giãn toàn màn hình y hệt trang sản phẩm
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

            // 2. Danh sách khách hàng
            grpCustomerList.Dock = DockStyle.Fill;
            grpCustomerList.Padding = new Padding(8, 22, 8, 8);
            dgvCustomers.Dock = DockStyle.Fill;

            // 3. Thanh phân trang
            var pager = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            pager.Controls.AddRange([btnPrev, lblPaging, btnNext]);

            // 4. Form nhập thông tin khách hàng
            grpCustomerInfo.Dock = DockStyle.Fill;
            grpCustomerInfo.Controls.Clear();

            var editor = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 4,
                Padding = new Padding(6)
            };
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            for (int row = 0; row < 3; row++)
            {
                editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            }
            editor.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Hàng 0: Mã KH & Tên KH
            lblCustomerId.Dock = txtCustomerId.Dock = lblCustomerName.Dock = txtCustomerName.Dock = DockStyle.Fill;
            lblCustomerId.Margin = lblCustomerName.Margin = new Padding(2, 7, 2, 2);
            editor.Controls.Add(lblCustomerId, 0, 0);
            editor.Controls.Add(txtCustomerId, 1, 0);
            editor.Controls.Add(lblCustomerName, 2, 0);
            editor.Controls.Add(txtCustomerName, 3, 0);

            // Hàng 1: Số điện thoại & Email
            lblPhone.Dock = txtPhone.Dock = lblEmail.Dock = txtEmail.Dock = DockStyle.Fill;
            lblPhone.Margin = lblEmail.Margin = new Padding(2, 7, 2, 2);
            editor.Controls.Add(lblPhone, 0, 1);
            editor.Controls.Add(txtPhone, 1, 1);
            editor.Controls.Add(lblEmail, 2, 1);
            editor.Controls.Add(txtEmail, 3, 1);

            // Hàng 2: Địa chỉ
            lblAddress.Dock = txtAddress.Dock = DockStyle.Fill;
            lblAddress.Margin = new Padding(2, 7, 2, 2);
            editor.Controls.Add(lblAddress, 0, 2);
            editor.Controls.Add(txtAddress, 1, 2);
            editor.SetColumnSpan(txtAddress, 3);

            // Hàng 3: Các nút chức năng
            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            foreach (var button in new[] { btnAdd, btnUpdate, btnDelete })
            {
                button.AutoSize = true;
                button.Margin = new Padding(6, 6, 6, 6);
                button.Padding = new Padding(10, 4, 10, 4);
                button.Cursor = Cursors.Hand;
                actions.Controls.Add(button);
            }
            editor.Controls.Add(actions, 0, 3);
            editor.SetColumnSpan(actions, 4);
            grpCustomerInfo.Controls.Add(editor);

            Controls.Clear();
            layout.Controls.Add(search, 0, 0);
            layout.Controls.Add(grpCustomerList, 0, 1);
            layout.Controls.Add(pager, 0, 2);
            layout.Controls.Add(grpCustomerInfo, 0, 3);
            Controls.Add(layout);

            ResumeLayout(true);
        }

        private void ConfigureGrid()
        {
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCustomers.ColumnHeadersHeight = 40;
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 10.5f);
            dgvCustomers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCustomers.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
            dgvCustomers.RowTemplate.Height = 44;

            dgvCustomers.Resize += (_, _) => FormProductManagement.AdjustRowHeights(dgvCustomers, PageSize);
        }

        // Tải danh sách khách hàng có phân trang
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
                    $"customers/paged?keyword={keyword}&pageNumber={page}&pageSize={PageSize}");

                if (!await CheckResponseAsync(response))
                    return;

                var result = await response.Content.ReadFromJsonAsync<PagedViewResult<CustomerItemViewDto>>();
                if (result == null || IsDisposed) return;

                if (result.TotalPages > 0 && page > result.TotalPages)
                {
                    await LoadDataAsync(result.TotalPages);
                    return;
                }

                _currentPage = result.TotalItems == 0 ? 1 : result.CurrentPage;
                _totalPages = result.TotalPages;

                var customers = result.Items.Select(x => new CustomerDto
                {
                    MaKhachHang = x.CustomerId,
                    HoTen = x.FullName,
                    SoDienThoai = x.Phone,
                    Email = x.Email,
                    DiaChi = x.Address,
                    DiemTichLuy = x.RewardPoints
                }).ToList();

                dgvCustomers.DataSource = null;
                dgvCustomers.AutoGenerateColumns = true;
                dgvCustomers.Columns.Clear();
                dgvCustomers.DataSource = customers;

                FormatDataGridView();
                FormProductManagement.AdjustRowHeights(dgvCustomers, PageSize);
                dgvCustomers.ClearSelection();
                ClearInputs();

                lblPaging.Text = $"Trang {(_totalPages == 0 ? 0 : _currentPage)} / {_totalPages} (Tổng: {result.TotalItems} khách hàng)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách khách hàng:\n" + ApiClientService.FriendlyError(ex),
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
            btnAdd.Enabled = btnUpdate.Enabled = !_busy && CanEditRole();
            btnDelete.Enabled = !_busy && IsAdminRole();
        }

        private bool CanEditRole() =>
            string.Equals(SessionManager.CurrentRole, "Admin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(SessionManager.CurrentRole, "Cashier", StringComparison.OrdinalIgnoreCase);

        private bool IsAdminRole() =>
            string.Equals(SessionManager.CurrentRole, "Admin", StringComparison.OrdinalIgnoreCase);

        private void FormatDataGridView()
        {
            if (dgvCustomers.Columns.Count == 0) return;

            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvCustomers.Columns["DiemTichLuy"] != null)
            {
                var col = dgvCustomers.Columns["DiemTichLuy"]!;
                col.HeaderText = "Điểm tích lũy";
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = 110;
                col.MinimumWidth = 100;
                col.DisplayIndex = dgvCustomers.Columns.Count - 1;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N0";
            }

            if (dgvCustomers.Columns["MaKhachHang"] != null)
            {
                var col = dgvCustomers.Columns["MaKhachHang"]!;
                col.HeaderText = "Mã KH";
                col.FillWeight = 12;
            }

            if (dgvCustomers.Columns["HoTen"] != null)
            {
                var col = dgvCustomers.Columns["HoTen"]!;
                col.HeaderText = "Họ và tên";
                col.FillWeight = 25;
            }

            if (dgvCustomers.Columns["SoDienThoai"] != null)
            {
                var col = dgvCustomers.Columns["SoDienThoai"]!;
                col.HeaderText = "Số điện thoại";
                col.FillWeight = 18;
            }

            if (dgvCustomers.Columns["Email"] != null)
            {
                var col = dgvCustomers.Columns["Email"]!;
                col.HeaderText = "Email";
                col.FillWeight = 22;
            }

            if (dgvCustomers.Columns["DiaChi"] != null)
            {
                var col = dgvCustomers.Columns["DiaChi"]!;
                col.HeaderText = "Địa chỉ";
                col.FillWeight = 28;
            }
        }

        private void ApplyPermissions()
        {
            bool canEdit = CanEditRole();
            bool isAdmin = IsAdminRole();
            btnAdd.Enabled = canEdit;
            btnUpdate.Enabled = canEdit;
            btnDelete.Enabled = isAdmin;
        }

        private bool CanEdit()
        {
            if (CanEditRole()) return true;
            MessageBox.Show("Bạn không có quyền thực hiện chức năng này!", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private bool IsAdmin()
        {
            if (IsAdminRole()) return true;
            MessageBox.Show("Chỉ Admin mới có quyền xóa khách hàng!", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
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

        private async Task<bool> CheckResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;

            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Phiên đăng nhập không hợp lệ hoặc đã hết hạn!",
                HttpStatusCode.Forbidden => "Tài khoản không có quyền thực hiện chức năng này!",
                HttpStatusCode.NotFound => "Không tìm thấy khách hàng!",
                HttpStatusCode.Conflict => "Số điện thoại đã tồn tại trong hệ thống!",
                _ => $"Yêu cầu thất bại! Mã HTTP: {(int)response.StatusCode}"
            };

            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void dgvCustomers_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCustomers.Rows.Count) return;
            var row = dgvCustomers.Rows[e.RowIndex];
            txtCustomerId.Text = row.Cells["MaKhachHang"].Value?.ToString() ?? string.Empty;
            txtCustomerName.Text = row.Cells["HoTen"].Value?.ToString() ?? string.Empty;
            txtPhone.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
            txtAddress.Text = row.Cells["DiaChi"].Value?.ToString() ?? string.Empty;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                txtCustomerName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!");
                txtPhone.Focus();
                return false;
            }

            string phone = txtPhone.Text.Trim();
            if (!Regex.IsMatch(phone, @"^\d{9,15}$"))
            {
                MessageBox.Show("Số điện thoại phải có từ 9 đến 15 chữ số!");
                txtPhone.Focus();
                return false;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Địa chỉ Email không hợp lệ!");
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ khách hàng!");
                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private object GetCustomerData() => new
        {
            HoTen = txtCustomerName.Text.Trim(),
            SoDienThoai = txtPhone.Text.Trim(),
            Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
            DiaChi = txtAddress.Text.Trim()
        };

        private async void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!CanEdit() || !ValidateInputs()) return;

            try
            {
                using var response = await SendRequestAsync(HttpMethod.Post, "customers", GetCustomerData());
                if (!await CheckResponseAsync(response)) return;

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm khách hàng:\n" + ApiClientService.FriendlyError(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button3_Click(object? sender, EventArgs e)
        {
            if (!CanEdit()) return;

            if (!int.TryParse(txtCustomerId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!");
                return;
            }

            if (!ValidateInputs()) return;

            try
            {
                using var response = await SendRequestAsync(HttpMethod.Put, $"customers/{id}", GetCustomerData());
                if (!await CheckResponseAsync(response)) return;

                MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật khách hàng:\n" + ApiClientService.FriendlyError(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (!IsAdmin()) return;

            if (!int.TryParse(txtCustomerId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng mã {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using var response = await SendRequestAsync(HttpMethod.Delete, $"customers/{id}");
                if (!await CheckResponseAsync(response)) return;

                MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync(_currentPage);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa khách hàng:\n" + ApiClientService.FriendlyError(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            dgvCustomers.ClearSelection();
        }

        private async void FormCustomerManagement_Load(object? sender, EventArgs e) => await LoadDataAsync(1);
    }

    public class CustomerItemViewDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Address { get; set; } = string.Empty;
        public int RewardPoints { get; set; }
    }

    public class CustomerDto
    {
        public int DiemTichLuy { get; set; }
        public int MaKhachHang { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string DiaChi { get; set; } = string.Empty;
    }
}
