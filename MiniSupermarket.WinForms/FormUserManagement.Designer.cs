namespace MiniSupermarket.WinForms
{
    partial class FormUserManagement
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
            inputPanel = new Panel();
            btnClear = new Button();
            btnLoad = new Button();
            btnToggleLock = new Button();
            btnResetPassword = new Button();
            btnAddUser = new Button();
            cboRole = new ComboBox();
            lblRole = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtFullName = new TextBox();
            lblFullName = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            dgvUsers = new DataGridView();
            mainLayout.SuspendLayout();
            inputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();
            //
            // mainLayout
            //
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(inputPanel, 0, 0);
            mainLayout.Controls.Add(dgvUsers, 0, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Padding = new Padding(16);
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 174F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            //
            // inputPanel
            //
            inputPanel.BackColor = Color.White;
            inputPanel.Controls.Add(btnClear);
            inputPanel.Controls.Add(btnLoad);
            inputPanel.Controls.Add(btnToggleLock);
            inputPanel.Controls.Add(btnResetPassword);
            inputPanel.Controls.Add(btnAddUser);
            inputPanel.Controls.Add(cboRole);
            inputPanel.Controls.Add(lblRole);
            inputPanel.Controls.Add(txtPassword);
            inputPanel.Controls.Add(lblPassword);
            inputPanel.Controls.Add(txtEmail);
            inputPanel.Controls.Add(lblEmail);
            inputPanel.Controls.Add(txtFullName);
            inputPanel.Controls.Add(lblFullName);
            inputPanel.Controls.Add(txtUsername);
            inputPanel.Controls.Add(lblUsername);
            inputPanel.Dock = DockStyle.Fill;
            //
            // btnClear
            //
            btnClear.Location = new Point(764, 118);
            btnClear.Size = new Size(104, 38);
            btnClear.Text = "Nhập mới";
            //
            // btnLoad
            //
            btnLoad.Location = new Point(876, 118);
            btnLoad.Size = new Size(104, 38);
            btnLoad.Text = "Tải lại";
            //
            // btnToggleLock
            //
            btnToggleLock.BackColor = Color.DarkOrange;
            btnToggleLock.FlatStyle = FlatStyle.Flat;
            btnToggleLock.ForeColor = Color.White;
            btnToggleLock.Location = new Point(450, 118);
            btnToggleLock.Size = new Size(160, 38);
            btnToggleLock.Text = "Khóa / Mở khóa";
            btnToggleLock.UseVisualStyleBackColor = false;
            //
            // btnResetPassword
            //
            btnResetPassword.BackColor = Color.SlateBlue;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(276, 118);
            btnResetPassword.Size = new Size(166, 38);
            btnResetPassword.Text = "Đặt lại mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            //
            // btnAddUser
            //
            btnAddUser.BackColor = Color.SeaGreen;
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(16, 118);
            btnAddUser.Size = new Size(145, 38);
            btnAddUser.Text = "Thêm tài khoản";
            btnAddUser.UseVisualStyleBackColor = false;
            //
            // cboRole
            //
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Location = new Point(764, 22);
            cboRole.Size = new Size(200, 28);
            //
            // lblRole
            //
            lblRole.AutoSize = true;
            lblRole.Location = new Point(764, 2);
            lblRole.Text = "Vai trò";
            //
            // txtPassword
            //
            txtPassword.Location = new Point(16, 78);
            txtPassword.Size = new Size(240, 27);
            txtPassword.UseSystemPasswordChar = true;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 56);
            lblPassword.Text = "Mật khẩu / Mật khẩu mới";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(520, 22);
            txtEmail.Size = new Size(230, 27);
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(520, 2);
            lblEmail.Text = "Email";
            //
            // txtFullName
            //
            txtFullName.Location = new Point(266, 22);
            txtFullName.Size = new Size(240, 27);
            //
            // lblFullName
            //
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(266, 2);
            lblFullName.Text = "Họ và tên";
            //
            // txtUsername
            //
            txtUsername.Location = new Point(16, 22);
            txtUsername.Size = new Size(240, 27);
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(16, 2);
            lblUsername.Text = "Tên đăng nhập";
            //
            // dgvUsers
            //
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.ColumnHeadersHeight = 38;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowTemplate.Height = 34;
            //
            // FormUserManagement
            //
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1050, 660);
            Controls.Add(mainLayout);
            Name = "FormUserManagement";
            Text = "Quản trị tài khoản";
            mainLayout.ResumeLayout(false);
            inputPanel.ResumeLayout(false);
            inputPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
        }

        private TableLayoutPanel mainLayout = null!;
        private Panel inputPanel = null!;
        private TextBox txtUsername = null!;
        private Label lblUsername = null!;
        private TextBox txtFullName = null!;
        private Label lblFullName = null!;
        private TextBox txtEmail = null!;
        private Label lblEmail = null!;
        private TextBox txtPassword = null!;
        private Label lblPassword = null!;
        private ComboBox cboRole = null!;
        private Label lblRole = null!;
        private Button btnAddUser = null!;
        private Button btnResetPassword = null!;
        private Button btnToggleLock = null!;
        private Button btnLoad = null!;
        private Button btnClear = null!;
        private DataGridView dgvUsers = null!;
    }
}
