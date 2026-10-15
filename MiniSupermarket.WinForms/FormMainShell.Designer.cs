namespace MiniSupermarket.WinForms
{
    partial class FormMainShell
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelSidebar = new Panel();
            panelSidebarFooter = new Panel();
            btnLogout = new Button();
            btnUserManage = new Button();
            btnReports = new Button();
            btnCustomer = new Button();
            btnProduct = new Button();
            btnCategory = new Button();
            btnPOS = new Button();
            lblLogo = new Label();
            panelTopHeader = new Panel();
            lblUserInfo = new Label();
            lblTitle = new Label();
            panelMainContent = new Panel();
            panelSidebar.SuspendLayout();
            panelSidebarFooter.SuspendLayout();
            panelTopHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(24, 30, 48);
            panelSidebar.Controls.Add(panelSidebarFooter);
            panelSidebar.Controls.Add(btnUserManage);
            panelSidebar.Controls.Add(btnReports);
            panelSidebar.Controls.Add(btnCustomer);
            panelSidebar.Controls.Add(btnProduct);
            panelSidebar.Controls.Add(btnCategory);
            panelSidebar.Controls.Add(btnPOS);
            panelSidebar.Controls.Add(lblLogo);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(230, 720);
            panelSidebar.TabIndex = 0;
            panelSidebar.Paint += panel1_Paint;
            // 
            // panelSidebarFooter
            // 
            panelSidebarFooter.Controls.Add(btnLogout);
            panelSidebarFooter.Dock = DockStyle.Bottom;
            panelSidebarFooter.Location = new Point(0, 660);
            panelSidebarFooter.Name = "panelSidebarFooter";
            panelSidebarFooter.Size = new Size(230, 60);
            panelSidebarFooter.TabIndex = 7;
            // 
            // btnLogout
            // 
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(10, 7);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(210, 45);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "🚪 Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // btnUserManage
            // 
            btnUserManage.FlatAppearance.BorderSize = 0;
            btnUserManage.FlatStyle = FlatStyle.Flat;
            btnUserManage.Font = new Font("Segoe UI", 10F);
            btnUserManage.ForeColor = Color.White;
            btnUserManage.Location = new Point(10, 340);
            btnUserManage.Name = "btnUserManage";
            btnUserManage.Size = new Size(210, 45);
            btnUserManage.TabIndex = 6;
            btnUserManage.Text = "🛡 Quản lý Tài khoản";
            btnUserManage.TextAlign = ContentAlignment.MiddleLeft;
            btnUserManage.UseVisualStyleBackColor = true;
            // 
            // btnReports
            // 
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 10F);
            btnReports.ForeColor = Color.White;
            btnReports.Location = new Point(10, 289);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(210, 45);
            btnReports.TabIndex = 5;
            btnReports.Text = "📊 Báo cáo Doanh thu";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = true;
            // 
            // btnCustomer
            // 
            btnCustomer.FlatAppearance.BorderSize = 0;
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.Font = new Font("Segoe UI", 10F);
            btnCustomer.ForeColor = Color.White;
            btnCustomer.Location = new Point(10, 240);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Size = new Size(210, 45);
            btnCustomer.TabIndex = 4;
            btnCustomer.Text = "👥 Quản lý Khách hàng";
            btnCustomer.TextAlign = ContentAlignment.MiddleLeft;
            btnCustomer.UseVisualStyleBackColor = true;
            // 
            // btnProduct
            // 
            btnProduct.FlatAppearance.BorderSize = 0;
            btnProduct.FlatStyle = FlatStyle.Flat;
            btnProduct.Font = new Font("Segoe UI", 10F);
            btnProduct.ForeColor = Color.White;
            btnProduct.Location = new Point(10, 190);
            btnProduct.Name = "btnProduct";
            btnProduct.Size = new Size(210, 45);
            btnProduct.TabIndex = 3;
            btnProduct.Text = "📦 Quản lý Sản phẩm";
            btnProduct.TextAlign = ContentAlignment.MiddleLeft;
            btnProduct.UseVisualStyleBackColor = true;
            // 
            // btnCategory
            // 
            btnCategory.FlatAppearance.BorderSize = 0;
            btnCategory.FlatStyle = FlatStyle.Flat;
            btnCategory.Font = new Font("Segoe UI", 10F);
            btnCategory.ForeColor = Color.White;
            btnCategory.Location = new Point(10, 140);
            btnCategory.Name = "btnCategory";
            btnCategory.Size = new Size(210, 45);
            btnCategory.TabIndex = 2;
            btnCategory.Text = "📁 Quản lý Nhóm hàng";
            btnCategory.TextAlign = ContentAlignment.MiddleLeft;
            btnCategory.UseVisualStyleBackColor = true;
            // 
            // btnPOS
            // 
            btnPOS.FlatAppearance.BorderSize = 0;
            btnPOS.FlatStyle = FlatStyle.Flat;
            btnPOS.ForeColor = Color.White;
            btnPOS.Location = new Point(10, 90);
            btnPOS.Name = "btnPOS";
            btnPOS.Size = new Size(210, 45);
            btnPOS.TabIndex = 1;
            btnPOS.Text = "\U0001f6d2 Bán hàng (POS)";
            btnPOS.TextAlign = ContentAlignment.MiddleLeft;
            btnPOS.UseVisualStyleBackColor = true;
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.BackColor = Color.Transparent;
            lblLogo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(20, 25);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(210, 32);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "ĐỜI SỐNG XANH";
            // 
            // panelTopHeader
            // 
            panelTopHeader.BackColor = Color.White;
            panelTopHeader.Controls.Add(lblUserInfo);
            panelTopHeader.Controls.Add(lblTitle);
            panelTopHeader.Dock = DockStyle.Top;
            panelTopHeader.Location = new Point(230, 0);
            panelTopHeader.Name = "panelTopHeader";
            panelTopHeader.Size = new Size(1050, 60);
            panelTopHeader.TabIndex = 1;
            // 
            // lblUserInfo
            // 
            lblUserInfo.AutoSize = true;
            lblUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserInfo.BackColor = Color.FromArgb(244, 245, 247);
            lblUserInfo.Font = new Font("Segoe UI", 10F);
            lblUserInfo.ForeColor = Color.Black;
            lblUserInfo.Location = new Point(760, 21);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(97, 23);
            lblUserInfo.TabIndex = 0;
            lblUserInfo.Text = "Xin chào: ...";
            lblUserInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblUserInfo.Click += panelMainContent_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(24, 30, 48);
            lblTitle.Location = new Point(0, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(314, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÀN LÀM VIỆC HỆ THỐNG";
            lblTitle.Click += lblTitle_Click;
            // 
            // panelMainContent
            // 
            panelMainContent.BackColor = Color.FromArgb(244, 245, 247);
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Location = new Point(230, 60);
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Size = new Size(1050, 660);
            panelMainContent.TabIndex = 2;
            panelMainContent.Paint += panelMainContent_Paint;
            // 
            // FormMainShell
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(panelMainContent);
            Controls.Add(panelTopHeader);
            Controls.Add(panelSidebar);
            Name = "FormMainShell";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống Quản lý Bán lẻ & Tồn kho Siêu thị Mini";
            panelSidebar.ResumeLayout(false);
            panelSidebar.PerformLayout();
            panelSidebarFooter.ResumeLayout(false);
            panelTopHeader.ResumeLayout(false);
            panelTopHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelTopHeader;
        private Label lblTitle;
        private Label lblUserInfo;
        private Panel panelMainContent;
        private Label lblLogo;
        private Button btnPOS;
        private Button btnUserManage;
        private Button btnReports;
        private Button btnCustomer;
        private Button btnProduct;
        private Button btnCategory;
        private Panel panelSidebarFooter;
        private Button btnLogout;
    }
}
