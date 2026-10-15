namespace MiniSupermarket.WinForms
{
    partial class FormQuickReport
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            mainLayout = new TableLayoutPanel();
            filterPanel = new Panel();
            lblReportDate = new Label();
            btnRunReport = new Button();
            dtpReportDate = new DateTimePicker();
            lblDateCaption = new Label();
            cardsLayout = new TableLayoutPanel();
            panelOrders = new Panel();
            lblTotalOrders = new Label();
            lblOrdersCaption = new Label();
            panelRevenue = new Panel();
            lblTotalRevenue = new Label();
            lblRevenueCaption = new Label();
            panelBestSeller = new Panel();
            lblBestSeller = new Label();
            lblBestSellerCaption = new Label();
            mainLayout.SuspendLayout();
            filterPanel.SuspendLayout();
            cardsLayout.SuspendLayout();
            panelOrders.SuspendLayout();
            panelRevenue.SuspendLayout();
            panelBestSeller.SuspendLayout();
            SuspendLayout();
            //
            // mainLayout
            //
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(filterPanel, 0, 0);
            mainLayout.Controls.Add(cardsLayout, 0, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Padding = new Padding(16);
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            //
            // filterPanel
            //
            filterPanel.BackColor = Color.White;
            filterPanel.Controls.Add(lblReportDate);
            filterPanel.Controls.Add(btnRunReport);
            filterPanel.Controls.Add(dtpReportDate);
            filterPanel.Controls.Add(lblDateCaption);
            filterPanel.Dock = DockStyle.Fill;
            //
            // lblReportDate
            //
            lblReportDate.AutoSize = true;
            lblReportDate.ForeColor = Color.DimGray;
            lblReportDate.Location = new Point(16, 58);
            lblReportDate.Text = "Chọn ngày và nhấn Xem báo cáo";
            //
            // btnRunReport
            //
            btnRunReport.BackColor = Color.FromArgb(41, 100, 180);
            btnRunReport.FlatStyle = FlatStyle.Flat;
            btnRunReport.ForeColor = Color.White;
            btnRunReport.Location = new Point(310, 18);
            btnRunReport.Size = new Size(145, 36);
            btnRunReport.Text = "Xem báo cáo";
            btnRunReport.UseVisualStyleBackColor = false;
            //
            // dtpReportDate
            //
            dtpReportDate.Format = DateTimePickerFormat.Short;
            dtpReportDate.Location = new Point(116, 21);
            dtpReportDate.Size = new Size(180, 27);
            //
            // lblDateCaption
            //
            lblDateCaption.AutoSize = true;
            lblDateCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDateCaption.Location = new Point(16, 24);
            lblDateCaption.Text = "Ngày báo cáo";
            //
            // cardsLayout
            //
            cardsLayout.ColumnCount = 3;
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            cardsLayout.Controls.Add(panelOrders, 0, 0);
            cardsLayout.Controls.Add(panelRevenue, 1, 0);
            cardsLayout.Controls.Add(panelBestSeller, 2, 0);
            cardsLayout.Dock = DockStyle.Fill;
            cardsLayout.Padding = new Padding(0, 16, 0, 0);
            //
            // panelOrders
            //
            panelOrders.BackColor = Color.FromArgb(41, 100, 180);
            panelOrders.Controls.Add(lblTotalOrders);
            panelOrders.Controls.Add(lblOrdersCaption);
            panelOrders.Dock = DockStyle.Fill;
            panelOrders.Margin = new Padding(0, 0, 12, 0);
            //
            // lblTotalOrders
            //
            lblTotalOrders.Dock = DockStyle.Fill;
            lblTotalOrders.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTotalOrders.ForeColor = Color.White;
            lblTotalOrders.Text = "0";
            lblTotalOrders.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblOrdersCaption
            //
            lblOrdersCaption.Dock = DockStyle.Top;
            lblOrdersCaption.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOrdersCaption.ForeColor = Color.White;
            lblOrdersCaption.Height = 60;
            lblOrdersCaption.Text = "TỔNG SỐ HÓA ĐƠN";
            lblOrdersCaption.TextAlign = ContentAlignment.MiddleCenter;
            //
            // panelRevenue
            //
            panelRevenue.BackColor = Color.SeaGreen;
            panelRevenue.Controls.Add(lblTotalRevenue);
            panelRevenue.Controls.Add(lblRevenueCaption);
            panelRevenue.Dock = DockStyle.Fill;
            panelRevenue.Margin = new Padding(6, 0, 6, 0);
            //
            // lblTotalRevenue
            //
            lblTotalRevenue.Dock = DockStyle.Fill;
            lblTotalRevenue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalRevenue.ForeColor = Color.White;
            lblTotalRevenue.Text = "0 đ";
            lblTotalRevenue.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblRevenueCaption
            //
            lblRevenueCaption.Dock = DockStyle.Top;
            lblRevenueCaption.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRevenueCaption.ForeColor = Color.White;
            lblRevenueCaption.Height = 60;
            lblRevenueCaption.Text = "TỔNG DOANH THU";
            lblRevenueCaption.TextAlign = ContentAlignment.MiddleCenter;
            //
            // panelBestSeller
            //
            panelBestSeller.BackColor = Color.DarkOrange;
            panelBestSeller.Controls.Add(lblBestSeller);
            panelBestSeller.Controls.Add(lblBestSellerCaption);
            panelBestSeller.Dock = DockStyle.Fill;
            panelBestSeller.Margin = new Padding(12, 0, 0, 0);
            //
            // lblBestSeller
            //
            lblBestSeller.Dock = DockStyle.Fill;
            lblBestSeller.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblBestSeller.ForeColor = Color.White;
            lblBestSeller.Text = "Chưa có dữ liệu";
            lblBestSeller.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblBestSellerCaption
            //
            lblBestSellerCaption.Dock = DockStyle.Top;
            lblBestSellerCaption.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBestSellerCaption.ForeColor = Color.White;
            lblBestSellerCaption.Height = 60;
            lblBestSellerCaption.Text = "MẶT HÀNG BÁN CHẠY NHẤT";
            lblBestSellerCaption.TextAlign = ContentAlignment.MiddleCenter;
            //
            // FormQuickReport
            //
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1050, 660);
            Controls.Add(mainLayout);
            Name = "FormQuickReport";
            Text = "Báo cáo doanh thu nhanh";
            mainLayout.ResumeLayout(false);
            filterPanel.ResumeLayout(false);
            filterPanel.PerformLayout();
            cardsLayout.ResumeLayout(false);
            panelOrders.ResumeLayout(false);
            panelRevenue.ResumeLayout(false);
            panelBestSeller.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TableLayoutPanel mainLayout = null!;
        private Panel filterPanel = null!;
        private Label lblDateCaption = null!;
        private DateTimePicker dtpReportDate = null!;
        private Button btnRunReport = null!;
        private Label lblReportDate = null!;
        private TableLayoutPanel cardsLayout = null!;
        private Panel panelOrders = null!;
        private Label lblOrdersCaption = null!;
        private Label lblTotalOrders = null!;
        private Panel panelRevenue = null!;
        private Label lblRevenueCaption = null!;
        private Label lblTotalRevenue = null!;
        private Panel panelBestSeller = null!;
        private Label lblBestSellerCaption = null!;
        private Label lblBestSeller = null!;
    }
}
