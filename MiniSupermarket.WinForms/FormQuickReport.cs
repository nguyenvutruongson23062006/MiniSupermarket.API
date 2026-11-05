using System.Net.Http.Json;
using System.Windows.Forms.DataVisualization.Charting;

namespace MiniSupermarket.WinForms;

public partial class FormQuickReport : Form
{
    private readonly DateTimePicker _from = new() { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today.AddDays(-6) };
    private readonly DateTimePicker _to = new() { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today };
    private readonly Button _load = new() { Text = "Xem báo cáo", AutoSize = true };
    private readonly Label[] _values = Enumerable.Range(0, 4).Select(_ => new Label
        { Text = "—", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 19, FontStyle.Bold), ForeColor = Color.FromArgb(30, 80, 140), TextAlign = ContentAlignment.MiddleCenter }).ToArray();
    private readonly Chart _chart = new() { Dock = DockStyle.Fill };
    private readonly DataGridView _best = new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
        AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AutoGenerateColumns = false };
    private readonly CancellationTokenSource _lifetime = new();
    private bool _loading;

    public FormQuickReport()
    {
        InitializeComponent();
        DoubleBuffered = true;
        Controls.Clear();
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(10) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var cards = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
        string[] captions = { "Doanh thu hôm nay", "Tổng hóa đơn đã xuất", "Khách mới hôm nay", "Mặt hàng tồn ≤ 10" };
        for (int i = 0; i < 4; i++)
        {
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.AliceBlue, Margin = new Padding(5) };
            card.Controls.Add(_values[i]);
            card.Controls.Add(new Label { Text = captions[i], Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter });
            cards.Controls.Add(card, i, 0);
        }
        var filters = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0, 8, 0, 0) };
        var preset = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };
        preset.Items.AddRange(new object[] { "Hôm nay", "Tuần này", "Tháng này", "Tùy chọn" });
        preset.SelectedIndex = 3;
        preset.SelectedIndexChanged += (_, _) =>
        {
            var today = DateTime.Today;
            if (preset.SelectedIndex == 0) _from.Value = today;
            if (preset.SelectedIndex == 1) _from.Value = today.AddDays(-((int)today.DayOfWeek + 6) % 7);
            if (preset.SelectedIndex == 2) _from.Value = new DateTime(today.Year, today.Month, 1);
            if (preset.SelectedIndex != 3) _to.Value = today;
        };
        filters.Controls.AddRange(new Control[] { preset, new Label { Text = "Từ ngày", AutoSize = true, Padding = new Padding(0, 7, 0, 0) }, _from,
            new Label { Text = "Đến ngày", AutoSize = true, Padding = new Padding(0, 7, 0, 0) }, _to, _load });
        var exportProducts = new Button { Text = "Xuất danh mục CSV", AutoSize = true };
        var exportOrders = new Button { Text = "Xuất hóa đơn CSV", AutoSize = true };
        filters.Controls.AddRange(new Control[] { exportProducts, exportOrders });
        exportProducts.Click += async (_, _) => await ExportAsync("reports/export-products", "DanhMucSanPham.csv");
        exportOrders.Click += async (_, _) => { if (ValidDates()) await ExportAsync("reports/export-orders?" + RangeQuery(), "HoaDon.csv"); };
        _chart.ChartAreas.Add(new ChartArea("Revenue"));
        _chart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
        _chart.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
        _chart.ChartAreas[0].AxisY.Title = "Doanh thu thực thu (VNĐ)";
        _chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        _chart.Series.Add(new Series("Doanh thu") { ChartType = SeriesChartType.Column, XValueType = ChartValueType.DateTime,
            Color = Color.SteelBlue, ToolTip = "#VALX{dd/MM/yyyy}: #VALY{N0} đ" });
        _chart.Titles.Add("Doanh thu theo ngày");
        var tabs = new TabControl { Dock = DockStyle.Fill };
        var chartTab = new TabPage("Biểu đồ doanh thu"); chartTab.Controls.Add(_chart);
        var bestTab = new TabPage("Top 5 bán chạy"); bestTab.Controls.Add(_best);
        foreach (var (property, caption) in new[] { ("ProductId", "Mã SP"), ("ProductName", "Tên sản phẩm"), ("TotalQuantity", "Số lượng đã bán"), ("GrossSales", "Tiền hàng trước giảm điểm") })
            _best.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = caption });
        _best.Columns[3].DefaultCellStyle.Format = "N0";
        tabs.TabPages.AddRange(new[] { chartTab, bestTab });
        layout.Controls.Add(cards, 0, 0); layout.Controls.Add(filters, 0, 1); layout.Controls.Add(tabs, 0, 2); Controls.Add(layout);
        _load.Click += async (_, _) => await LoadReportAsync();
        Load += async (_, _) => await LoadReportAsync();
        Disposed += (_, _) => { _lifetime.Cancel(); _lifetime.Dispose(); };
    }

    private bool ValidDates()
    {
        if (_from.Value.Date <= _to.Value.Date && (_to.Value.Date - _from.Value.Date).TotalDays <= 366) return true;
        MessageBox.Show("Ngày bắt đầu không được sau ngày kết thúc. Chọn tối đa 366 ngày.", "Khoảng ngày không hợp lệ");
        return false;
    }
    private string RangeQuery() => $"fromDate={_from.Value:yyyy-MM-dd}&toDate={_to.Value:yyyy-MM-dd}";
    private async Task<T> ReadAsync<T>(string endpoint)
    {
        using var response = await ApiClientService.Client.GetAsync(endpoint, _lifetime.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: _lifetime.Token)
            ?? throw new System.Text.Json.JsonException();
    }
    private async Task LoadReportAsync()
    {
        if (_loading || !ValidDates()) return;
        _loading = true; _load.Enabled = false;
        try
        {
            var summary = await ReadAsync<DashboardSummaryDto>("reports/dashboard-summary");
            var days = await ReadAsync<List<DailyRevenueView>>("reports/daily-revenue?" + RangeQuery());
            var best = await ReadAsync<List<BestSellerView>>("reports/best-sellers?" + RangeQuery());
            if (IsDisposed) return;
            _values[0].Text = $"{summary.RevenueToday:N0} đ";
            _values[1].Text = summary.TotalOrders.ToString("N0");
            _values[2].Text = summary.NewCustomersToday.ToString("N0");
            _values[3].Text = summary.LowStockProducts.ToString("N0");
            _values[3].ForeColor = summary.LowStockProducts > 0 ? Color.Firebrick : Color.DarkGreen;
            _chart.Series[0].Points.Clear();
            foreach (var day in days) _chart.Series[0].Points.AddXY(day.Date, day.Revenue);
            _chart.Titles[0].Text = $"Doanh thu {_from.Value:dd/MM/yyyy} – {_to.Value:dd/MM/yyyy}";
            _best.DataSource = best;
        }
        catch (OperationCanceledException) when (IsDisposed || Disposing) { }
        catch (Exception ex) { if (!IsDisposed) MessageBox.Show(ApiClientService.FriendlyError(ex), "Không tải được báo cáo"); }
        finally { _loading = false; if (!IsDisposed) _load.Enabled = true; }
    }
    private async Task ExportAsync(string endpoint, string filename)
    {
        try
        {
            using var response = await ApiClientService.Client.GetAsync(endpoint, _lifetime.Token);
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync(_lifetime.Token);
            if (IsDisposed) return;
            using var dialog = new SaveFileDialog { Filter = "CSV (mở bằng Excel)|*.csv", FileName = filename, DefaultExt = "csv", AddExtension = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            await File.WriteAllBytesAsync(dialog.FileName, bytes, _lifetime.Token);
            if (!IsDisposed) MessageBox.Show("Đã xuất báo cáo thành công.", "Xuất CSV");
        }
        catch (OperationCanceledException) when (IsDisposed || Disposing) { }
        catch (IOException) { if (!IsDisposed) MessageBox.Show("Không ghi được tệp. Hãy đóng tệp trong Excel và kiểm tra thư mục lưu.", "Xuất CSV"); }
        catch (Exception ex) { if (!IsDisposed) MessageBox.Show(ApiClientService.FriendlyError(ex), "Xuất CSV"); }
    }
}
public class DashboardSummaryDto { public decimal RevenueToday { get; set; } public int TotalOrders { get; set; } public int NewCustomersToday { get; set; } public int LowStockProducts { get; set; } }
public class DailyRevenueView { public DateTime Date { get; set; } public decimal Revenue { get; set; } }
public class BestSellerView { public int ProductId { get; set; } public string ProductName { get; set; } = ""; public int TotalQuantity { get; set; } public decimal GrossSales { get; set; } }
