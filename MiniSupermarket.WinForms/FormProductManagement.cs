
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormProductManagement : Form
    {
        // Địa chỉ API của dự án
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7163/api/")
        };

        private readonly BindingSource _productsSource = new();

        // Khởi tạo Form và các sự kiện
        public FormProductManagement()
        {
            InitializeComponent();

            // Đăng ký sự kiện cho các nút Toolbox
            btnSearch.Click += async (s, e) => await SearchProductsAsync();
            btnLoad.Click += async (s, e) => await LoadProductsAsync();
            btnAdd.Click += async (s, e) => await AddProductAsync();
            btnUpdate.Click += async (s, e) => await UpdateProductAsync();
            btnDelete.Click += async (s, e) => await DeleteProductAsync();

            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            ConfigureGrid();
            ApplyPermissions();
        }

        // Kiểm tra quyền người dùng
        private void ApplyPermissions()
        {
            string role = SessionManager.CurrentRole ?? "";

            bool canManage =
                role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("Warehouse", StringComparison.OrdinalIgnoreCase);

            btnAdd.Enabled = canManage;
            btnUpdate.Enabled = canManage;
            btnDelete.Enabled = canManage;
        }

        // Cấu hình các cột của DataGridView
        private void ConfigureGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            dgvProducts.ReadOnly = true;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            AddColumn("MaSanPham", "Mã SP");
            AddColumn("MaVach", "Mã vạch");
            AddColumn("TenSanPham", "Tên sản phẩm");
            AddColumn("GiaBan", "Giá bán");
            AddColumn("SoLuongTon", "Số lượng tồn");
            AddColumn("TenNhomHang", "Nhóm hàng");

            dgvProducts.DataSource = _productsSource;
        }

        // Thêm cột dữ liệu
        private void AddColumn(string property, string title)
        {
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = property,
                DataPropertyName = property,
                HeaderText = title
            });
        }

        // Tạo HTTP request kèm JWT Token
        private async Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string endpoint,
            object? body = null)
        {
            using var request =
                new HttpRequestMessage(method, endpoint);

            if (!string.IsNullOrWhiteSpace(SessionManager.JwtToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        SessionManager.JwtToken);
            }

            if (body != null)
                request.Content = JsonContent.Create(body);

            return await _http.SendAsync(request);
        }

        // Kiểm tra kết quả từ API
        private async Task<bool> CheckResponseAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized =>
                    "Phiên đăng nhập không hợp lệ hoặc đã hết hạn.",

                HttpStatusCode.Forbidden =>
                    "Bạn không có quyền thực hiện chức năng này.",

                HttpStatusCode.NotFound =>
                    "Không tìm thấy sản phẩm.",

                HttpStatusCode.Conflict =>
                    "Dữ liệu bị trùng hoặc sản phẩm đang được sử dụng.",

                _ => await response.Content.ReadAsStringAsync()
            };

            MessageBox.Show(
                message,
                $"Lỗi HTTP {(int)response.StatusCode}",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        // Khi Form mở: tải nhóm hàng và sản phẩm
        private async void FormProductManagement_Load(
            object sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        // Lấy danh sách nhóm hàng từ API
        private async Task LoadCategoriesAsync()
        {
            try
            {
                using var response =
                    await SendAsync(HttpMethod.Get, "categories");

                if (!await CheckResponseAsync(response))
                    return;

                var categories = await response.Content
                    .ReadFromJsonAsync<List<ProductCategoryDto>>();

                cboCategory.DataSource =
                    categories ?? new List<ProductCategoryDto>();

                cboCategory.DisplayMember = "TenNhomHang";
                cboCategory.ValueMember = "MaNhomHang";
                cboCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Lấy toàn bộ sản phẩm từ API
        private async Task LoadProductsAsync()
        {
            try
            {
                using var response =
                    await SendAsync(HttpMethod.Get, "products");

                if (!await CheckResponseAsync(response))
                    return;

                var products = await response.Content
                    .ReadFromJsonAsync<List<ProductDto>>();

                _productsSource.DataSource =
                    products ?? new List<ProductDto>();

                dgvProducts.ClearSelection();
                ClearInputs();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Khi chọn một sản phẩm trong bảng
        private void DgvProducts_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem
                is not ProductDto product)
                return;

            txtProductId.Text = product.MaSanPham.ToString();
            txtBarcode.Text = product.MaVach;
            txtProductName.Text = product.TenSanPham;

            numPrice.Value = Math.Min(
                numPrice.Maximum,
                Math.Max(numPrice.Minimum, product.GiaBan));

            numStock.Value = Math.Min(
                numStock.Maximum,
                Math.Max(numStock.Minimum, product.SoLuongTon));

            if (cboCategory.DataSource != null)
                cboCategory.SelectedValue = product.MaNhomHang;
        }

        // Kiểm tra và lấy dữ liệu nhập từ giao diện
        private ProductInputDto? GetInput()
        {
            string barcode = txtBarcode.Text.Trim();
            string name = txtProductName.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Vui lòng nhập mã vạch.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                return null;
            }

            if (cboCategory.SelectedValue is not int categoryId)
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng.");
                return null;
            }

            return new ProductInputDto
            {
                MaVach = barcode,
                TenSanPham = name,
                GiaBan = numPrice.Value,
                SoLuongTon = Decimal.ToInt32(numStock.Value),
                MaNhomHang = categoryId
            };
        }

        // Thêm sản phẩm
        private async Task AddProductAsync()
        {
            var input = GetInput();

            if (input == null)
                return;

            try
            {
                using var response =
                    await SendAsync(HttpMethod.Post, "products", input);

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show("Thêm sản phẩm thành công!");

                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Cập nhật sản phẩm
        private async Task UpdateProductAsync()
        {
            if (!int.TryParse(txtProductId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.");
                return;
            }

            var input = GetInput();

            if (input == null)
                return;

            try
            {
                using var response = await SendAsync(
                    HttpMethod.Put,
                    $"products/{id}",
                    input);

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show("Cập nhật sản phẩm thành công!");

                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Xóa sản phẩm
        private async Task DeleteProductAsync()
        {
            if (!int.TryParse(txtProductId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm mã {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using var response = await SendAsync(
                    HttpMethod.Delete,
                    $"products/{id}");

                if (!await CheckResponseAsync(response))
                    return;

                MessageBox.Show("Xóa sản phẩm thành công!");

                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Tìm kiếm theo tên hoặc mã vạch
        private async Task SearchProductsAsync()
        {
            string keyword = txtKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadProductsAsync();
                return;
            }

            try
            {
                string url = "products/search?keyword=" +
                    Uri.EscapeDataString(keyword);

                using var response =
                    await SendAsync(HttpMethod.Get, url);

                if (!await CheckResponseAsync(response))
                    return;

                var products = await response.Content
                    .ReadFromJsonAsync<List<ProductDto>>();

                _productsSource.DataSource =
                    products ?? new List<ProductDto>();

                dgvProducts.ClearSelection();
                ClearInputs();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // Xóa thông tin nhập
        private void ClearInputs()
        {
            txtProductId.Clear();
            txtBarcode.Clear();
            txtProductName.Clear();

            numPrice.Value = 0;
            numStock.Value = 0;
            cboCategory.SelectedIndex = -1;
        }

        // Hiển thị lỗi
        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Giữ lại sự kiện Designer đã tạo
        private void textBox2_TextChanged(
            object sender, EventArgs e)
        {
        }

        // Giữ lại sự kiện Designer đã tạo
        private void button1_Click(
            object sender, EventArgs e)
        {
        }

        // Giải phóng HttpClient
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _http.Dispose();
            base.OnFormClosed(e);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void lblCategory_Click(object sender, EventArgs e)
        {

        }
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
