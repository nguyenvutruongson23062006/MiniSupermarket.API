
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormMainShell : Form
    {
        // Lưu Form con đang hiển thị
        private Form? _activeForm = null;

        // Khởi tạo Form chính
        public FormMainShell()
        {
            InitializeComponent();

            // Đăng ký sự kiện khi Form được mở
            this.Load += FormMainShell_Load;

            // Đăng ký sự kiện các nút Sidebar
            btnPOS.Click += btnPOS_Click;
            btnCategory.Click += btnCategory_Click;
            btnProduct.Click += btnProduct_Click;
            btnCustomer.Click += btnCustomer_Click;
            btnReports.Click += btnReports_Click;
            btnUserManage.Click += btnUserManage_Click;
            btnLogout.Click += btnLogout_Click;
        }

        // =========================================
        // 1. LOAD FORM CHÍNH
        // =========================================
        private void FormMainShell_Load(
            object? sender,
            EventArgs e)
        {
            // Hiển thị thông tin tài khoản
            lblUserInfo.Text =
                $"Nhân viên: {SessionManager.CurrentUsername} | " +
                $"Vai trò: [{SessionManager.CurrentRole}]";

            // Phân quyền theo vai trò
            ApplyRolePermissions(
                SessionManager.CurrentRole);

            // Mở màn hình mặc định
            OpenDefaultScreenByRole(
                SessionManager.CurrentRole);
        }

        // =========================================
        // 2. MỞ FORM CON TRONG PANEL
        // =========================================
        private void OpenChildForm(
            Form childForm,
            string screenTitle,
            Button senderButton)
        {
            // Đóng Form đang hiển thị
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm = null;
            }

            // Làm nổi bật nút Sidebar
            HighlightActiveButton(senderButton);

            // Thiết lập Form con
            _activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Xóa nội dung cũ
            panelMainContent.Controls.Clear();

            // Thêm Form con vào Panel
            panelMainContent.Controls.Add(childForm);
            panelMainContent.Tag = childForm;

            // Cập nhật tiêu đề
            lblTitle.Text = screenTitle;

            childForm.BringToFront();
            childForm.Show();
        }

        // =========================================
        // 3. LÀM NỔI BẬT NÚT SIDEBAR
        // =========================================
        private void HighlightActiveButton(
            Button activeButton)
        {
            // Đặt lại màu các nút
            foreach (Control ctrl in panelSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor =
                        Color.FromArgb(24, 30, 48);
                }
            }

            // Đổi màu nút đang hoạt động
            activeButton.BackColor =
                Color.FromArgb(41, 100, 180);
        }

        // =========================================
        // 4. PHÂN QUYỀN GIAO DIỆN
        // =========================================
        private void ApplyRolePermissions(string role)
        {
            switch ((role ?? "").ToUpperInvariant())
            {
                // Admin sử dụng tất cả chức năng
                case "ADMIN":
                    btnPOS.Visible = true;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = true;
                    btnReports.Visible = true;
                    btnUserManage.Visible = true;
                    break;

                // Cashier: Bán hàng và Khách hàng
                case "CASHIER":
                    btnPOS.Visible = true;
                    btnCategory.Visible = false;
                    btnProduct.Visible = false;
                    btnCustomer.Visible = true;
                    btnReports.Visible = false;
                    btnUserManage.Visible = false;
                    break;

                // Warehouse: Nhóm hàng và Sản phẩm
                case "WAREHOUSE":
                    btnPOS.Visible = false;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = false;
                    btnReports.Visible = false;
                    btnUserManage.Visible = false;
                    break;

                // Vai trò không hợp lệ
                default:
                    MessageBox.Show(
                        "Tài khoản chưa được cấp quyền hợp lệ!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.BeginInvoke(
                        new Action(() => this.Close()));
                    break;
            }
        }

        // =========================================
        // 5. MỞ MÀN HÌNH MẶC ĐỊNH THEO VAI TRÒ
        // =========================================
        private void OpenDefaultScreenByRole(string role)
        {
            switch ((role ?? "").ToUpperInvariant())
            {
                // Admin mở quản lý nhóm hàng
                case "ADMIN":
                    OpenChildForm(
                        new FormCategoryManagement(),
                        "QUẢN LÝ NHÓM HÀNG",
                        btnCategory);
                    break;

                // Cashier mở bán hàng POS
                case "CASHIER":
                    OpenChildForm(
                        new FormPOS(),
                        "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)",
                        btnPOS);
                    break;

                // Warehouse mở quản lý sản phẩm
                case "WAREHOUSE":
                    OpenChildForm(
                        new FormProductManagement(),
                        "QUẢN LÝ SẢN PHẨM",
                        btnProduct);
                    break;
            }
        }

        // =========================================
        // 7. NÚT BÁN HÀNG POS
        // =========================================
        private void btnPOS_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanAccess("ADMIN", "CASHIER"))
                return;

            OpenChildForm(
                new FormPOS(),
                "HỆ THỐNG QUẦY BÁN HÀNG & THU NGÂN (POS)",
                btnPOS);
        }

        // =========================================
        // 8. NÚT QUẢN LÝ NHÓM HÀNG
        // =========================================
        private void btnCategory_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanAccess("ADMIN", "WAREHOUSE"))
                return;

            OpenChildForm(
                new FormCategoryManagement(),
                "QUẢN LÝ NHÓM HÀNG",
                btnCategory);
        }

        // =========================================
        // 9. NÚT QUẢN LÝ SẢN PHẨM
        // =========================================
        private void btnProduct_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanAccess("ADMIN", "WAREHOUSE"))
                return;

            OpenChildForm(
                new FormProductManagement(),
                "QUẢN LÝ SẢN PHẨM",
                btnProduct);
        }

        // =========================================
        // 10. NÚT QUẢN LÝ KHÁCH HÀNG - ĐÃ SỬA
        // =========================================
        private void btnCustomer_Click(
            object? sender,
            EventArgs e)
        {
            // Chỉ Admin và Cashier được truy cập
            if (!CanAccess("ADMIN", "CASHIER"))
                return;

            // Mở Form Quản lý Khách hàng
            // trong panelMainContent
            OpenChildForm(
                new FormCustomerManagement(),
                "QUẢN LÝ KHÁCH HÀNG",
                btnCustomer);
        }

        // =========================================
        // 11. NÚT BÁO CÁO DOANH THU
        // =========================================
        private void btnReports_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanAccess("ADMIN"))
                return;

            OpenChildForm(
                new FormQuickReport(),
                "BÁO CÁO DOANH THU & HIỆU SUẤT CA BÁN",
                btnReports);
        }

        // =========================================
        // 12. NÚT QUẢN LÝ TÀI KHOẢN
        // =========================================
        private void btnUserManage_Click(
            object? sender,
            EventArgs e)
        {
            if (!CanAccess("ADMIN"))
                return;

            OpenChildForm(
                new FormUserManagement(),
                "QUẢN TRỊ TÀI KHOẢN VÀ PHÂN QUYỀN HỆ THỐNG",
                btnUserManage);
        }

        // =========================================
        // 13. KIỂM TRA QUYỀN TRUY CẬP
        // =========================================
        private bool CanAccess(
            params string[] allowedRoles)
        {
            string currentRole =
                SessionManager.CurrentRole
                ?? string.Empty;

            foreach (string role in allowedRoles)
            {
                if (string.Equals(
                    currentRole,
                    role,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            MessageBox.Show(
                "Bạn không có quyền truy cập chức năng này!",
                "Không đủ quyền",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        // =========================================
        // 14. NÚT ĐĂNG XUẤT
        // =========================================
        private void btnLogout_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            // Xóa thông tin phiên đăng nhập
            SessionManager.JwtToken = string.Empty;
            SessionManager.CurrentUsername = string.Empty;
            SessionManager.CurrentRole = string.Empty;

            // Đóng Form chính để quay lại FormLogin
            this.Close();
        }

        // =========================================
        // 15. CÁC SỰ KIỆN CŨ CỦA DESIGNER
        // Giữ nguyên để tránh lỗi
        // =========================================
        private void panel1_Paint(
            object sender,
            PaintEventArgs e)
        {
        }

        private void panelMainContent_Click(
            object sender,
            EventArgs e)
        {
        }

        private void lblTitle_Click(
            object sender,
            EventArgs e)
        {
        }

        private void button1_Click(
            object sender,
            EventArgs e)
        {
        }

        private void button2_Click(
            object sender,
            EventArgs e)
        {
        }

        private void panelMainContent_Paint(
            object sender,
            PaintEventArgs e)
        {
        }
    }
}
