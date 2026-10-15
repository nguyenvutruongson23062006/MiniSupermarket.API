using System.Net;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormPOS : Form
    {
        private readonly List<PosCartItem> _cart = new();
        private int? _selectedCustomerId;

        public FormPOS()
        {
            InitializeComponent();
            ConfigureCartGrid();

            splitMain.Resize +=
                (_, _) => ResizeSplitLayout();
            paymentPanel.Resize +=
                (_, _) => ResizePaymentLayout();

            ResizeSplitLayout();
            ResizePaymentLayout();

            txtBarcode.KeyDown += txtBarcode_KeyDown;
            txtCashReceived.TextChanged +=
                (_, _) => CalculateChange();
            btnFindCustomer.Click +=
                async (_, _) => await FindCustomerAsync();
            btnRemoveItem.Click +=
                (_, _) => RemoveSelectedItem();
            btnClearCart.Click +=
                (_, _) => ClearCart();
            btnCheckout.Click +=
                async (_, _) => await CheckoutAsync();
            Load += (_, _) => txtBarcode.Focus();
            KeyDown += FormPOS_KeyDown;
        }

        // Giữ khu vực thanh toán gọn ở bên phải, để bảng sản phẩm
        // luôn có đủ chiều rộng khi shell được phóng to/thu nhỏ.
        private void ResizeSplitLayout()
        {
            const int paymentWidth = 390;

            if (splitMain.Width <= 0)
                return;

            int splitterDistance =
                splitMain.Width - paymentWidth - splitMain.SplitterWidth;

            splitterDistance = Math.Max(
                splitMain.Panel1MinSize,
                splitterDistance);

            splitMain.SplitterDistance = splitterDistance;
        }

        // Căn các ô nhập và số tiền theo đúng chiều rộng panel phải.
        private void ResizePaymentLayout()
        {
            if (paymentPanel.ClientSize.Width <= 0)
                return;

            int contentWidth =
                Math.Max(220, paymentPanel.ClientSize.Width - 40);

            txtCustomerPhone.Width = Math.Max(
                120,
                contentWidth - btnFindCustomer.Width - 8);
            btnFindCustomer.Left =
                txtCustomerPhone.Right + 8;

            lblCustomerName.Width = contentWidth;
            lblTotalAmount.Width = contentWidth;
            txtCashReceived.Width = contentWidth;
            lblChange.Width = contentWidth;
            btnCheckout.Width = contentWidth;
            btnClearCart.Width = contentWidth;
        }

        private void ConfigureCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.MultiSelect = false;
            dgvCart.ReadOnly = true;
            dgvCart.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PosCartItem.MaSanPham),
                HeaderText = "Mã SP",
                Width = 70
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PosCartItem.TenSanPham),
                HeaderText = "Tên sản phẩm",
                AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PosCartItem.DonGia),
                HeaderText = "Đơn giá",
                Width = 105,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment =
                        DataGridViewContentAlignment.MiddleRight
                }
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PosCartItem.SoLuong),
                HeaderText = "SL",
                Width = 55,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment =
                        DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PosCartItem.ThanhTien),
                HeaderText = "Thành tiền",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment =
                        DataGridViewContentAlignment.MiddleRight
                }
            });

            BindCart();
        }

        private async void txtBarcode_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            string barcode = txtBarcode.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcode))
                return;

            txtBarcode.Clear();
            await AddProductByBarcodeAsync(barcode);
        }

        private async Task AddProductByBarcodeAsync(
            string barcode)
        {
            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Get,
                    $"products/barcode/{Uri.EscapeDataString(barcode)}");

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show(
                        "Không tìm thấy sản phẩm có mã vạch này!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (!await EnsureSuccessAsync(response))
                    return;

                var product = await response.Content
                    .ReadFromJsonAsync<ProductDto>();

                if (product == null)
                {
                    MessageBox.Show("Dữ liệu sản phẩm không hợp lệ.");
                    return;
                }

                var existing = _cart.FirstOrDefault(x =>
                    x.MaSanPham == product.MaSanPham);

                if (existing != null)
                {
                    if (existing.SoLuong >= product.SoLuongTon)
                    {
                        MessageBox.Show(
                            $"'{product.TenSanPham}' chỉ còn " +
                            $"{product.SoLuongTon} sản phẩm trong kho.",
                            "Không đủ tồn kho",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    existing.SoLuong++;
                }
                else
                {
                    if (product.SoLuongTon <= 0)
                    {
                        MessageBox.Show(
                            "Sản phẩm đã hết hàng.",
                            "Không đủ tồn kho",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    _cart.Add(new PosCartItem
                    {
                        MaSanPham = product.MaSanPham,
                        MaVach = product.MaVach,
                        TenSanPham = product.TenSanPham,
                        DonGia = product.GiaBan,
                        SoLuong = 1,
                        SoLuongTon = product.SoLuongTon
                    });
                }

                BindCart();
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
            finally
            {
                txtBarcode.Focus();
            }
        }

        private async Task FindCustomerAsync()
        {
            string phone = txtCustomerPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                _selectedCustomerId = null;
                lblCustomerName.Text = "Khách vãng lai";
                return;
            }

            try
            {
                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Get,
                    "customers/search?keyword=" +
                    Uri.EscapeDataString(phone));

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                var customers = await response.Content
                    .ReadFromJsonAsync<List<CustomerDto>>()
                    ?? new List<CustomerDto>();

                var customer = customers.FirstOrDefault(x =>
                    string.Equals(
                        x.SoDienThoai,
                        phone,
                        StringComparison.OrdinalIgnoreCase));

                if (customer == null)
                {
                    _selectedCustomerId = null;
                    lblCustomerName.Text = "Không tìm thấy khách hàng";
                    MessageBox.Show(
                        "Không tìm thấy khách hàng theo số điện thoại. " +
                        "Bạn có thể để trống để bán cho khách vãng lai.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                _selectedCustomerId = customer.MaKhachHang;
                lblCustomerName.Text =
                    $"{customer.HoTen} - Mã KH: {customer.MaKhachHang}";
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
        }

        private void RemoveSelectedItem()
        {
            if (dgvCart.CurrentRow?.DataBoundItem
                is not PosCartItem item)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần bỏ.");
                return;
            }

            if (item.SoLuong > 1)
                item.SoLuong--;
            else
                _cart.Remove(item);

            BindCart();
        }

        private async Task CheckoutAsync()
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show(
                    "Giỏ hàng đang trống!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            decimal total = _cart.Sum(x => x.ThanhTien);

            if (!decimal.TryParse(
                txtCashReceived.Text,
                out decimal cashReceived) ||
                cashReceived < total)
            {
                MessageBox.Show(
                    "Tiền khách đưa chưa đủ để thanh toán.",
                    "Kiểm tra thanh toán",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            var checkoutData = new
            {
                MaKhachHang = _selectedCustomerId,
                SoDienThoaiKhachHang = _selectedCustomerId.HasValue
                    ? null
                    : NullIfEmpty(txtCustomerPhone.Text),
                SanPhams = _cart.Select(x => new
                {
                    x.MaSanPham,
                    x.SoLuong
                }).ToList()
            };

            try
            {
                btnCheckout.Enabled = false;

                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Post,
                    "orders/checkout");
                request.Content = JsonContent.Create(checkoutData);

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!await EnsureSuccessAsync(response))
                    return;

                var result = await response.Content
                    .ReadFromJsonAsync<CheckoutResultDto>();

                MessageBox.Show(
                    "Thanh toán thành công!\n" +
                    $"Mã hóa đơn: {result?.MaDonHang}\n" +
                    $"Tổng tiền: {total:N0} đ\n" +
                    $"Tiền thừa: {cashReceived - total:N0} đ",
                    "Hoàn tất",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearCart();
                txtCustomerPhone.Clear();
                _selectedCustomerId = null;
                lblCustomerName.Text = "Khách vãng lai";
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
            finally
            {
                btnCheckout.Enabled = true;
                txtBarcode.Focus();
            }
        }

        private static string? NullIfEmpty(string value)
        {
            value = value.Trim();
            return value.Length == 0 ? null : value;
        }

        private async Task<bool> EnsureSuccessAsync(
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
                    "Tài khoản không có quyền thực hiện chức năng này.",
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

        private void BindCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart.ToList();
            lblTotalAmount.Text =
                $"{_cart.Sum(x => x.ThanhTien):N0} đ";
            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal total = _cart.Sum(x => x.ThanhTien);

            if (decimal.TryParse(
                txtCashReceived.Text,
                out decimal cashReceived))
            {
                decimal change = cashReceived - total;
                lblChange.Text = change >= 0
                    ? $"{change:N0} đ"
                    : "Chưa đủ tiền!";
                lblChange.ForeColor = change >= 0
                    ? Color.DarkGreen
                    : Color.Firebrick;
            }
            else
            {
                lblChange.Text = "0 đ";
                lblChange.ForeColor = Color.Black;
            }
        }

        private void ClearCart()
        {
            _cart.Clear();
            txtCashReceived.Clear();
            BindCart();
        }

        private async void FormPOS_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F9)
                return;

            e.SuppressKeyPress = true;
            await CheckoutAsync();
        }

        private static void ShowConnectionError(Exception ex)
        {
            MessageBox.Show(
                "Không thể kết nối Web API:\n" + ex.Message,
                "Lỗi kết nối",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void paymentPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class PosCartItem
    {
        public int MaSanPham { get; set; }
        public string MaVach { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongTon { get; set; }
        public decimal ThanhTien => DonGia * SoLuong;
    }

    public class CheckoutResultDto
    {
        public int MaDonHang { get; set; }
        public decimal TongTien { get; set; }
    }
}
