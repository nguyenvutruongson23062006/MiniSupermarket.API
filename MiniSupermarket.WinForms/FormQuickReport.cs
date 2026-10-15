using System.Net;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormQuickReport : Form
    {
        public FormQuickReport()
        {
            InitializeComponent();
            dtpReportDate.Value = DateTime.Today;
            btnRunReport.Click +=
                async (_, _) => await LoadReportAsync();
            Load += async (_, _) => await LoadReportAsync();
        }

        private async Task LoadReportAsync()
        {
            if (!string.Equals(
                SessionManager.CurrentRole,
                "Admin",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Bạn không có quyền xem dữ liệu tài chính của siêu thị!",
                    "Từ chối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            try
            {
                btnRunReport.Enabled = false;
                string date = dtpReportDate.Value
                    .ToString("yyyy-MM-dd");

                using var request = ApiClientService.CreateRequest(
                    HttpMethod.Get,
                    $"reports/quick?date={date}");

                using var response =
                    await ApiClientService.Client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(response);
                    return;
                }

                var report = await response.Content
                    .ReadFromJsonAsync<QuickReportDto>();

                if (report == null)
                {
                    MessageBox.Show("Dữ liệu báo cáo không hợp lệ.");
                    return;
                }

                lblTotalOrders.Text =
                    report.TongSoHoaDon.ToString("N0");
                lblTotalRevenue.Text =
                    $"{report.TongDoanhThu:N0} đ";
                lblBestSeller.Text =
                    report.SoLuongMatHangBanChay > 0
                        ? $"{report.MatHangBanChayNhat}\n" +
                          $"Đã bán: {report.SoLuongMatHangBanChay:N0}"
                        : "Chưa có dữ liệu";
                lblReportDate.Text =
                    $"Kết quả ngày {report.NgayBaoCao:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải báo cáo:\n" + ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRunReport.Enabled = true;
            }
        }

        private static async Task ShowApiErrorAsync(
            HttpResponseMessage response)
        {
            string detail = await response.Content.ReadAsStringAsync();
            string message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized =>
                    "Phiên đăng nhập đã hết hạn.",
                HttpStatusCode.Forbidden =>
                    "Chỉ Admin được xem báo cáo doanh thu.",
                _ => string.IsNullOrWhiteSpace(detail)
                    ? $"API trả về mã {(int)response.StatusCode}."
                    : detail
            };

            MessageBox.Show(
                message,
                "Lỗi Web API",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public class QuickReportDto
    {
        public DateTime NgayBaoCao { get; set; }
        public int TongSoHoaDon { get; set; }
        public decimal TongDoanhThu { get; set; }
        public string MatHangBanChayNhat { get; set; } =
            string.Empty;
        public int SoLuongMatHangBanChay { get; set; }
    }
}
