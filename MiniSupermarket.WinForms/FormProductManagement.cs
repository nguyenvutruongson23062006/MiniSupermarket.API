using System.Net;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormProductManagement : Form
    {
        private readonly BindingSource _productsSource = new();
        private readonly Button btnPrev = new() { Name = "btnPrev", Text = "<< Trang trước", AutoSize = true };
        private readonly Button btnNext = new() { Name = "btnNext", Text = "Trang sau >>", AutoSize = true };
        private readonly Label lblPaging = new() { Name = "lblPaging", AutoSize = true, Padding = new Padding(8) };
        private readonly ComboBox cboCategoryFilter = new()
        {
            Name = "cboCategoryFilter", DropDownStyle = ComboBoxStyle.DropDownList,
            AccessibleName = "Lọc nhóm hàng", Dock = DockStyle.Fill
        };
        private const int PageSize = 5;
        private int _currentPage = 1;
        private int _totalPages;
        private bool _busy;
        private bool _categoriesLoaded;
        private bool _binding;

        public FormProductManagement()
        {
            InitializeComponent();
            ConfigureLayout();
            ConfigureGrid();
            btnSearch.Click += async (_, _) => await LoadProductsAsync(1);
            btnUpdate.Click += async (_, _) => await UpdateProductAsync();
            btnDelete.Click += async (_, _) => await DeleteProductAsync();
            btnPrev.Click += async (_, _) => await LoadProductsAsync(_currentPage - 1);
            btnNext.Click += async (_, _) => await LoadProductsAsync(_currentPage + 1);
            cboCategoryFilter.SelectedIndexChanged += async (_, _) =>
            {
                if (_categoriesLoaded) await LoadProductsAsync(1);
            };
            txtKeyword.KeyDown += async (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                await LoadProductsAsync(1);
            };
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            UpdateButtons();
        }

        // Dùng lại các control Designer, bố trí co giãn theo vùng nội dung của shell.
        private void ConfigureLayout()
        {
            SuspendLayout();
            AutoScroll = false;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(12) };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 215));
            var search = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            search.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 205));
            txtKeyword.Dock = DockStyle.Fill;
            btnSearch.Dock = btnLoad.Dock = DockStyle.Fill;
            search.Controls.Add(txtKeyword, 0, 0);
            search.Controls.Add(btnSearch, 1, 0);
            search.Controls.Add(btnLoad, 2, 0);
            search.Controls.Add(cboCategoryFilter, 3, 0);
            var pager = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            pager.Controls.AddRange([btnPrev, lblPaging, btnNext]);
            grpProductList1.Dock = DockStyle.Fill;
            dgvProducts.Dock = DockStyle.Fill;
            grpProductList1.Padding = new Padding(8, 22, 8, 8);
            grpProductList2.Dock = DockStyle.Fill;
            grpProductList2.Controls.Clear();
            var editor = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, Padding = new Padding(6) };
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int row = 0; row < 3; row++) editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            editor.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Control[] labels = [lblProductId, lblBarcode, lblProductName, lblPrice, lblStock, lblCategory];
            Control[] inputs = [txtProductId, txtBarcode, txtProductName, numPrice, numStock, cboCategory];
            for (int i = 0; i < inputs.Length; i++)
            {
                int col = (i % 2) * 2, row = i / 2;
                labels[i].Dock = inputs[i].Dock = DockStyle.Fill;
                labels[i].Margin = new Padding(2, 7, 2, 2);
                editor.Controls.Add(labels[i], col, row);
                editor.Controls.Add(inputs[i], col + 1, row);
            }
            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            foreach (var button in new[] { btnAdd, btnUpdate, btnDelete })
            {
                button.AutoSize = true;
                button.Margin = new Padding(4);
                actions.Controls.Add(button);
            }
            editor.Controls.Add(actions, 0, 3);
            editor.SetColumnSpan(actions, 4);
            grpProductList2.Controls.Add(editor);
            Controls.Clear();
            layout.Controls.Add(search, 0, 0);
            layout.Controls.Add(grpProductList1, 0, 1);
            layout.Controls.Add(pager, 0, 2);
            layout.Controls.Add(grpProductList2, 0, 3);
            Controls.Add(layout);
            ResumeLayout(true);
        }

        private void ConfigureGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();
            AddColumn("MaSanPham", "Mã SP");
            AddColumn("MaVach", "Mã vạch");
            AddColumn("TenSanPham", "Tên sản phẩm");
            AddColumn("GiaBan", "Giá bán");
            AddColumn("SoLuongTon", "Tồn kho");
            AddColumn("TenNhomHang", "Nhóm hàng");
            dgvProducts.Columns["TenSanPham"].FillWeight = 180;
            dgvProducts.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            dgvProducts.DataSource = _productsSource;
        }

        private void AddColumn(string property, string title) =>
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            { Name = property, DataPropertyName = property, HeaderText = title });

        private void UpdateButtons()
        {
            bool canManage = SessionManager.CurrentRole is "Admin" or "Warehouse";
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = !_busy && canManage;
            btnSearch.Enabled = btnLoad.Enabled = txtKeyword.Enabled = cboCategoryFilter.Enabled = !_busy;
            grpProductList2.Enabled = !_busy;
            dgvProducts.Enabled = !_busy;
            btnPrev.Enabled = !_busy && _currentPage > 1;
            btnNext.Enabled = !_busy && _currentPage < _totalPages;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string endpoint, object? body = null)
        {
            using var request = ApiClientService.CreateRequest(method, endpoint);
            if (body != null) request.Content = JsonContent.Create(body);
            return await ApiClientService.Client.SendAsync(request);
        }

        private async Task<bool> CheckResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return true;
            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Phiên đăng nhập không hợp lệ hoặc đã hết hạn.",
                HttpStatusCode.Forbidden => "Bạn không có quyền thực hiện chức năng này.",
                _ => await response.Content.ReadAsStringAsync()
            };
            MessageBox.Show(message, $"Lỗi HTTP {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private async void FormProductManagement_Load(object sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync(1);
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                using var response = await SendAsync(HttpMethod.Get, "categories");
                if (!await CheckResponseAsync(response)) return;
                var categories = await response.Content.ReadFromJsonAsync<List<ProductCategoryDto>>() ?? new();
                cboCategory.DisplayMember = nameof(ProductCategoryDto.TenNhomHang);
                cboCategory.ValueMember = nameof(ProductCategoryDto.MaNhomHang);
                cboCategory.DataSource = categories;
                cboCategory.SelectedIndex = -1;
                cboCategoryFilter.DisplayMember = nameof(ProductCategoryDto.TenNhomHang);
                cboCategoryFilter.ValueMember = nameof(ProductCategoryDto.MaNhomHang);
                cboCategoryFilter.DataSource = new List<ProductCategoryDto>
                { new() { MaNhomHang = 0, TenNhomHang = "Tất cả nhóm hàng" } }.Concat(categories).ToList();
                _categoriesLoaded = true;
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async Task LoadProductsAsync(int page)
        {
            if (_busy || IsDisposed) return;
            _busy = true;
            UpdateButtons();
            try
            {
                page = Math.Max(1, page);
                string keyword = Uri.EscapeDataString(txtKeyword.Text.Trim());
                int category = cboCategoryFilter.SelectedValue is int id ? id : 0;
                async Task<PagedViewResult<ProductItemViewDto>?> Fetch(int number)
                {
                    using var response = await SendAsync(HttpMethod.Get,
                        $"products/paged?keyword={keyword}&categoryId={category}&pageNumber={number}&pageSize={PageSize}");
                    if (!await CheckResponseAsync(response)) return null;
                    return await response.Content.ReadFromJsonAsync<PagedViewResult<ProductItemViewDto>>();
                }
                var result = await Fetch(page);
                if (result == null || IsDisposed) return;
                // Khi xóa dòng cuối trang cuối, lùi về trang cuối còn dữ liệu.
                if (result.TotalPages > 0 && page > result.TotalPages)
                    result = await Fetch(result.TotalPages);
                if (result == null || IsDisposed) return;
                _currentPage = result.TotalItems == 0 ? 1 : result.CurrentPage;
                _totalPages = result.TotalPages;
                _binding = true;
                _productsSource.DataSource = result.Items.Select(p => new ProductDto
                {
                    MaSanPham = p.ProductId, MaVach = p.Barcode, TenSanPham = p.ProductName,
                    GiaBan = p.Price, SoLuongTon = p.StockQuantity,
                    MaNhomHang = p.CategoryId, TenNhomHang = p.CategoryName
                }).ToList();
                dgvProducts.ClearSelection();
                ClearInputs();
                lblPaging.Text = $"Trang {(_totalPages == 0 ? 0 : _currentPage)} / {_totalPages} (Tổng: {result.TotalItems} SP)";
            }
            catch (Exception ex) { if (!IsDisposed) ShowError(ex); }
            finally
            {
                _binding = false;
                _busy = false;
                if (!IsDisposed) UpdateButtons();
            }
        }

        private void DgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (_binding || dgvProducts.SelectedRows.Count == 0 || dgvProducts.CurrentRow?.DataBoundItem is not ProductDto p) return;
            txtProductId.Text = p.MaSanPham.ToString();
            txtBarcode.Text = p.MaVach;
            txtProductName.Text = p.TenSanPham;
            numPrice.Value = Math.Clamp(p.GiaBan, numPrice.Minimum, numPrice.Maximum);
            numStock.Value = Math.Clamp(p.SoLuongTon, numStock.Minimum, numStock.Maximum);
            cboCategory.SelectedValue = p.MaNhomHang;
        }

        private ProductInputDto? GetInput()
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text) || string.IsNullOrWhiteSpace(txtProductName.Text))
            { MessageBox.Show("Vui lòng nhập mã vạch và tên sản phẩm."); return null; }
            if (cboCategory.SelectedValue is not int categoryId)
            { MessageBox.Show("Vui lòng chọn nhóm hàng."); return null; }
            return new ProductInputDto
            {
                MaVach = txtBarcode.Text.Trim(), TenSanPham = txtProductName.Text.Trim(),
                GiaBan = numPrice.Value, SoLuongTon = decimal.ToInt32(numStock.Value), MaNhomHang = categoryId
            };
        }

        private async Task SaveAsync(HttpMethod method, string endpoint, object? body, string message)
        {
            if (_busy) return;
            bool success = false;
            _busy = true;
            UpdateButtons();
            try
            {
                using var response = await SendAsync(method, endpoint, body);
                success = await CheckResponseAsync(response);
                if (success) MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ShowError(ex); }
            finally { _busy = false; if (!IsDisposed) UpdateButtons(); }
            if (success && !IsDisposed) await LoadProductsAsync(_currentPage);
        }

        private async Task AddProductAsync()
        {
            if (_busy) return;
            var input = GetInput();
            if (input != null) await SaveAsync(HttpMethod.Post, "products", input, "Thêm sản phẩm thành công!");
        }

        private async Task UpdateProductAsync()
        {
            if (_busy) return;
            if (!int.TryParse(txtProductId.Text, out int id))
            { MessageBox.Show("Vui lòng chọn sản phẩm cần sửa."); return; }
            var input = GetInput();
            if (input != null) await SaveAsync(HttpMethod.Put, $"products/{id}", input, "Cập nhật sản phẩm thành công!");
        }

        private async Task DeleteProductAsync()
        {
            if (_busy) return;
            if (!int.TryParse(txtProductId.Text, out int id))
            { MessageBox.Show("Vui lòng chọn sản phẩm cần xóa."); return; }
            if (MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm mã {id}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            await SaveAsync(HttpMethod.Delete, $"products/{id}", null, "Xóa sản phẩm thành công!");
        }

        private void ClearInputs()
        {
            txtProductId.Clear(); txtBarcode.Clear(); txtProductName.Clear();
            numPrice.Value = numStock.Value = 0;
            cboCategory.SelectedIndex = -1;
        }

        private static void ShowError(Exception ex) => MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // Hai sự kiện này được Designer đăng ký, không đăng ký lần thứ hai trong constructor.
        private async void button1_Click(object sender, EventArgs e) => await AddProductAsync();
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            txtKeyword.Clear();
            _categoriesLoaded = false;
            cboCategoryFilter.SelectedIndex = 0;
            _categoriesLoaded = true;
            await LoadProductsAsync(1);
        }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox1_Enter_1(object sender, EventArgs e) { }
    }

    public class PagedViewResult<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
    }

    public class ProductItemViewDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    // Dữ liệu sản phẩm trả về từ API
    public class ProductDto
    {
        public int MaSanPham { get; set; }
        public string MaVach { get; set; } = "";
        public string TenSanPham { get; set; } = "";
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public int MaNhomHang { get; set; }
        public string TenNhomHang { get; set; } = "";
    }

    // Dữ liệu gửi lên API khi thêm/sửa
    public class ProductInputDto
    {
        public string MaVach { get; set; } = "";
        public string TenSanPham { get; set; } = "";
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public int MaNhomHang { get; set; }
    }

    // Dữ liệu nhóm hàng
    public class ProductCategoryDto
    {
        public int MaNhomHang { get; set; }
        public string TenNhomHang { get; set; } = "";
        public string? MoTa { get; set; }
    }
}
